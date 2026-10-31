#nullable enable
using System;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microting.KanbanBase.Infrastructure.Data;
using Microting.KanbanBase.Infrastructure.Data.Entities;
using Microting.KanbanBase.Infrastructure.Enums;
using NUnit.Framework;

namespace Microting.KanbanBase.Tests;

/// <summary>Sub-project C (spec 2026-10-30 §4.1): workspaces, members, customers, invitations; board, project and key scoping.</summary>
[TestFixture]
public class WorkspacesSchemaTests
{
    private const string EntityNamespace = "Microting.KanbanBase.Infrastructure.Data.Entities";

    // A model-only context: an explicit server version, so nothing connects to a database.
    private static KanbanPnDbContext Context() => new(new DbContextOptionsBuilder<KanbanPnDbContext>()
        .UseMySql("Server=localhost;Database=kanban-model-only;user=root;password=unused",
            new MariaDbServerVersion(new Version(10, 8)))
        .Options);

    [Test]
    public void WorkspaceRole_Values_AreFixed()
    {
        Assert.Multiple(() =>
        {
            Assert.That((int)WorkspaceRole.Admin, Is.EqualTo(1));
            Assert.That((int)WorkspaceRole.Team, Is.EqualTo(2));
            Assert.That((int)WorkspaceRole.Customer, Is.EqualTo(3));
        });
    }

    [TestCase(typeof(Workspace), typeof(WorkspaceVersion))]
    [TestCase(typeof(WorkspaceMember), typeof(WorkspaceMemberVersion))]
    [TestCase(typeof(Customer), typeof(CustomerVersion))]
    [TestCase(typeof(Invitation), typeof(InvitationVersion))]
    [TestCase(typeof(Board), typeof(BoardVersion))]
    [TestCase(typeof(Project), typeof(ProjectVersion))]
    [TestCase(typeof(IntegrationApiKey), typeof(IntegrationApiKeyVersion))]
    [TestCase(typeof(CardTenant), typeof(CardTenantVersion))]
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
    public void Model_SeedsWorkspaceOne()
    {
        using var ctx = Context();
        // Seed data lives only in the design-time model; the runtime (read-optimized) model throws on GetSeedData.
        var seed = ctx.GetService<IDesignTimeModel>().Model.FindEntityType(typeof(Workspace))!.GetSeedData().Single();
        Assert.Multiple(() =>
        {
            Assert.That(seed["Id"], Is.EqualTo(1));
            Assert.That(seed["Name"], Is.EqualTo("Microting"));
            Assert.That(seed["Slug"], Is.EqualTo("microting"));
            Assert.That(seed["WorkflowState"], Is.EqualTo("created"));
        });
    }

    [TestCase(typeof(Board))]
    [TestCase(typeof(Project))]
    [TestCase(typeof(IntegrationApiKey))]
    public void Model_WorkspaceId_IsARequiredRestrictForeignKey(Type entity)
    {
        using var ctx = Context();
        var fk = ctx.Model.FindEntityType(entity)!.GetForeignKeys()
            .Single(f => f.PrincipalEntityType.ClrType == typeof(Workspace));
        Assert.Multiple(() =>
        {
            Assert.That(fk.Properties.Single().Name, Is.EqualTo("WorkspaceId"));
            Assert.That(fk.IsRequired, Is.True);
            Assert.That(fk.DeleteBehavior, Is.EqualTo(DeleteBehavior.Restrict));
        });
    }

    [Test]
    public void Model_CardTenantCustomerId_IsOptional()
    {
        using var ctx = Context();
        var fk = ctx.Model.FindEntityType(typeof(CardTenant))!.GetForeignKeys()
            .Single(f => f.PrincipalEntityType.ClrType == typeof(Customer));
        Assert.That(fk.IsRequired, Is.False);
    }

    [Test]
    public void Model_Lengths_And_Indexes()
    {
        using var ctx = Context();
        var ws = ctx.Model.FindEntityType(typeof(Workspace))!;
        var inv = ctx.Model.FindEntityType(typeof(Invitation))!;
        var key = ctx.Model.FindEntityType(typeof(IntegrationApiKey))!;
        Assert.Multiple(() =>
        {
            Assert.That(ws.FindProperty("Name")!.GetMaxLength(), Is.EqualTo(255));
            Assert.That(ws.FindProperty("Slug")!.GetMaxLength(), Is.EqualTo(64));
            Assert.That(ws.GetIndexes().Single(i => i.Properties.Single().Name == "Slug").IsUnique, Is.True);
            Assert.That(inv.FindProperty("TokenHash")!.GetMaxLength(), Is.EqualTo(64));
            Assert.That(inv.GetIndexes().Single(i => i.Properties.Count == 1 && i.Properties[0].Name == "TokenHash").IsUnique, Is.True);
            Assert.That(inv.FindProperty("Email")!.GetMaxLength(), Is.EqualTo(255));
            Assert.That(key.FindProperty("Scope")!.GetMaxLength(), Is.EqualTo(256));
            // Live-row uniqueness of (WorkspaceId, UserId) is enforced in code (soft delete); the index is not unique.
            Assert.That(ctx.Model.FindEntityType(typeof(WorkspaceMember))!.GetIndexes()
                .Single(i => i.Properties.Select(p => p.Name).SequenceEqual(new[] { "WorkspaceId", "UserId" })).IsUnique, Is.False);
        });
    }

    [Test]
    public void Migrations_IncludeWorkspaces_AndMatchTheModel()
    {
        using var ctx = Context();
        Assert.That(ctx.Database.GetMigrations(), Has.Some.EndsWith("_Workspaces"));
        Assert.That(ctx.Database.HasPendingModelChanges(), Is.False);
    }
}
