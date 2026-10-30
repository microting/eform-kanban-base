#nullable enable
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microting.KanbanBase.Infrastructure.Data;
using Microting.KanbanBase.Infrastructure.Data.Entities;
using NUnit.Framework;

namespace Microting.KanbanBase.Tests;

/// <summary>
/// The CardExternalIds migration: a card's origin outside kanban (support mail: "gmail", the Gmail message and thread
/// ids), the dedupe key of eform-kanban-plugin's email-messages endpoint (plugin spec 2026-10-28 §5).
/// </summary>
[TestFixture]
public class CardExternalIdsSchemaTests
{
    // A model-only context: an explicit server version, so nothing connects to a database.
    private static KanbanPnDbContext Context() => new(new DbContextOptionsBuilder<KanbanPnDbContext>()
        .UseMySql("Server=localhost;Database=kanban-model-only;user=root;password=unused",
            new MariaDbServerVersion(new Version(10, 8)))
        .Options);

    [Test]
    public void Card_ExternalFields_DefaultToNull()
    {
        var card = new Card();
        Assert.That(card.ExternalSource, Is.Null);
        Assert.That(card.ExternalId, Is.Null);
        Assert.That(card.ExternalThreadId, Is.Null);
    }

    [Test]
    public void CardVersion_HasTheExternalFields()
    {
        // KanbanPnBase.MapVersion copies by name and only logs a miss: the version twin must have all three.
        foreach (var name in new[] { nameof(Card.ExternalSource), nameof(Card.ExternalId), nameof(Card.ExternalThreadId) })
            Assert.That(typeof(CardVersion).GetProperty(name)?.PropertyType, Is.EqualTo(typeof(string)), name);
    }

    [Test]
    public void Model_ExternalFields_HaveTheirMaxLengths()
    {
        using var ctx = Context();
        var card = ctx.Model.FindEntityType(typeof(Card))!;
        Assert.Multiple(() =>
        {
            Assert.That(card.FindProperty(nameof(Card.ExternalSource))!.GetMaxLength(), Is.EqualTo(32));
            Assert.That(card.FindProperty(nameof(Card.ExternalId))!.GetMaxLength(), Is.EqualTo(128));
            Assert.That(card.FindProperty(nameof(Card.ExternalThreadId))!.GetMaxLength(), Is.EqualTo(128));
            Assert.That(card.FindProperty(nameof(Card.ExternalId))!.IsNullable, Is.True);
        });
    }

    [Test]
    public void Model_HasNonUniqueLookupIndexesPerBoard()
    {
        using var ctx = Context();
        var indexes = ctx.Model.FindEntityType(typeof(Card))!.GetIndexes()
            .Select(i => (string.Join(",", i.Properties.Select(p => p.Name)), i.IsUnique)).ToList();
        // Non-unique, like (BoardId, SentryIssueId): cards are soft-deleted and MySQL has no filtered indexes.
        Assert.That(indexes, Does.Contain(("BoardId,ExternalSource,ExternalId", false)));
        Assert.That(indexes, Does.Contain(("BoardId,ExternalSource,ExternalThreadId", false)));
    }

    [Test]
    public void Migrations_EndWithCardExternalIds_AndMatchTheModel()
    {
        using var ctx = Context();
        Assert.That(ctx.Database.GetMigrations().Last(), Does.EndWith("_CardExternalIds"));
        Assert.That(ctx.Database.HasPendingModelChanges(), Is.False);
    }
}
