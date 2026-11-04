#nullable enable
using System;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microting.KanbanBase.Infrastructure.Data;
using Microting.KanbanBase.Infrastructure.Data.Entities;
using Microting.KanbanBase.Infrastructure.Enums;
using NUnit.Framework;

namespace Microting.KanbanBase.Tests;

/// <summary>Sub-project A (spec 2026-10-30 §5, §7): notifications, followers, answer links and webhooks.</summary>
[TestFixture]
public class NotificationsSchemaTests
{
    private const string EntityNamespace = "Microting.KanbanBase.Infrastructure.Data.Entities";

    private static KanbanPnDbContext Context() => new(new DbContextOptionsBuilder<KanbanPnDbContext>()
        .UseMySql("Server=localhost;Database=kanban-model-only;user=root;password=unused",
            new MariaDbServerVersion(new Version(10, 8)))
        .Options);

    [Test]
    public void Enums_ArePersistedInts_AndFixed()
    {
        Assert.Multiple(() =>
        {
            Assert.That(Enum.GetValues<NotificationEvent>().Select(e => (int)e), Is.EqualTo(new[] { 1, 2, 3, 4, 5, 6 }));
            Assert.That(NotificationEvent.AgentQuestion.ToString(), Is.EqualTo("AgentQuestion"));
            Assert.That(NotificationEvent.StatusChange.ToString(), Is.EqualTo("StatusChange"));
            Assert.That(Enum.GetValues<NotificationChannel>().Select(e => (int)e), Is.EqualTo(new[] { 1, 2, 3 }));
            Assert.That((int)NotificationStatus.Pending, Is.EqualTo(0));
            Assert.That((int)NotificationStatus.Skipped, Is.EqualTo(5));
        });
    }

    [TestCase(typeof(NotificationPreference), typeof(NotificationPreferenceVersion))]
    [TestCase(typeof(NotificationQuietHours), typeof(NotificationQuietHoursVersion))]
    [TestCase(typeof(DeviceToken), typeof(DeviceTokenVersion))]
    [TestCase(typeof(CardFollower), typeof(CardFollowerVersion))]
    [TestCase(typeof(NotificationOutbox), typeof(NotificationOutboxVersion))]
    [TestCase(typeof(AnswerLink), typeof(AnswerLinkVersion))]
    [TestCase(typeof(WorkspaceWebhook), typeof(WorkspaceWebhookVersion))]
    public void VersionTwin_HasEveryScalar(Type entity, Type version)
    {
        // KanbanPnBase.MapVersion copies by name and only logs a miss.
        var scalars = entity.GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.Name != "Id" && p.PropertyType.Namespace != EntityNamespace
                        && !(p.PropertyType.IsGenericType && p.PropertyType.GetGenericArguments().Any(a => a.Namespace == EntityNamespace)));
        Assert.Multiple(() =>
        {
            foreach (var p in scalars)
                Assert.That(version.GetProperty(p.Name)?.PropertyType, Is.EqualTo(p.PropertyType), $"{version.Name}.{p.Name}");
            Assert.That(version.GetProperty(entity.Name + "Id")?.PropertyType, Is.EqualTo(typeof(int)));
        });
    }

    [Test]
    public void Defaults_AreTheSpecsDefaults()
    {
        Assert.Multiple(() =>
        {
            Assert.That(new NotificationOutbox().Count, Is.EqualTo(1));
            Assert.That(new NotificationOutbox().Status, Is.EqualTo(NotificationStatus.Pending));
            Assert.That(new NotificationQuietHours().TimeZone, Is.EqualTo("Europe/Copenhagen"));
            Assert.That(new NotificationQuietHours().FromMinute, Is.EqualTo(22 * 60));
            Assert.That(new NotificationQuietHours().ToMinute, Is.EqualTo(7 * 60));
            Assert.That(new NotificationQuietHours().Enabled, Is.False);
        });
    }

    /// <summary>Whether the one index over exactly <paramref name="columns"/> (in order) is unique; throws when there is none.</summary>
    private static bool Unique(KanbanPnDbContext ctx, Type entity, params string[] columns) =>
        ctx.Model.FindEntityType(entity)!.GetIndexes()
            .Single(i => i.Properties.Select(p => p.Name).SequenceEqual(columns)).IsUnique;

    [Test]
    public void Model_UniqueIndexes_GuardOneRowPerIdentity()
    {
        using var ctx = Context();
        Assert.Multiple(() =>
        {
            Assert.That(Unique(ctx, typeof(NotificationPreference), "UserId", "WorkspaceId", "Event"), Is.True);
            Assert.That(Unique(ctx, typeof(NotificationQuietHours), "UserId"), Is.True);
            Assert.That(Unique(ctx, typeof(DeviceToken), "AppId", "InstallationId"), Is.True);
            Assert.That(Unique(ctx, typeof(DeviceToken), "Token"), Is.False);
            Assert.That(Unique(ctx, typeof(CardFollower), "CardId", "UserId"), Is.True);
            Assert.That(Unique(ctx, typeof(AnswerLink), "CommentId"), Is.True);
            Assert.That(Unique(ctx, typeof(AnswerLink), "CardId", "TaskId"), Is.False);
            Assert.That(Unique(ctx, typeof(WorkspaceWebhook), "WorkspaceId"), Is.True);
            Assert.That(Unique(ctx, typeof(NotificationOutbox), "Status", "NextAttemptAt"), Is.False);
            Assert.That(Unique(ctx, typeof(NotificationOutbox), "RecipientUserId", "CardId", "Event", "Channel", "Status"), Is.False);
        });
    }

    [Test]
    public void Model_Lengths()
    {
        using var ctx = Context();
        int? Max(Type t, string p) => ctx.Model.FindEntityType(t)!.FindProperty(p)!.GetMaxLength();
        Assert.Multiple(() =>
        {
            Assert.That(Max(typeof(DeviceToken), "AppId"), Is.EqualTo(32));
            Assert.That(Max(typeof(DeviceToken), "InstallationId"), Is.EqualTo(128));
            Assert.That(Max(typeof(DeviceToken), "Platform"), Is.EqualTo(16));
            Assert.That(Max(typeof(DeviceToken), "Token"), Is.EqualTo(512));
            Assert.That(Max(typeof(DeviceToken), "AppVersion"), Is.EqualTo(32));
            Assert.That(Max(typeof(NotificationQuietHours), "TimeZone"), Is.EqualTo(64));
            Assert.That(Max(typeof(NotificationOutbox), "LastError"), Is.EqualTo(500));
            Assert.That(ctx.Model.FindEntityType(typeof(NotificationOutbox))!.FindProperty("Payload")!.GetColumnType(), Is.EqualTo("longtext"));
            Assert.That(Max(typeof(WorkspaceWebhook), "Url"), Is.EqualTo(1000));
            Assert.That(Max(typeof(WorkspaceWebhook), "SecretCipher"), Is.EqualTo(512));
            Assert.That(Max(typeof(WorkspaceWebhook), "SecretIv"), Is.EqualTo(64));
        });
    }

    [Test]
    public void Model_CardFollower_CascadesWithItsCard_WebhookRestrictsItsWorkspace()
    {
        using var ctx = Context();
        var follower = ctx.Model.FindEntityType(typeof(CardFollower))!.GetForeignKeys().Single(f => f.PrincipalEntityType.ClrType == typeof(Card));
        var webhook = ctx.Model.FindEntityType(typeof(WorkspaceWebhook))!.GetForeignKeys().Single(f => f.PrincipalEntityType.ClrType == typeof(Workspace));
        Assert.That(follower.DeleteBehavior, Is.EqualTo(DeleteBehavior.Cascade));
        Assert.That(webhook.DeleteBehavior, Is.EqualTo(DeleteBehavior.Restrict));
    }

    [Test]
    public void Migrations_IncludeNotificationsAndAnswers_AndMatchTheModel()
    {
        using var ctx = Context();
        Assert.That(ctx.Database.GetMigrations(), Has.Some.EndsWith("_NotificationsAndAnswers"));
        Assert.That(ctx.Database.HasPendingModelChanges(), Is.False);
    }
}
