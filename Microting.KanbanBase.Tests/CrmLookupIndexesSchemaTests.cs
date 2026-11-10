#nullable enable
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microting.KanbanBase.Infrastructure.Data;
using Microting.KanbanBase.Infrastructure.Data.Entities;
using NUnit.Framework;

namespace Microting.KanbanBase.Tests;

/// <summary>
/// The CrmLookupIndexes migration: the CRM lists a company's cards by customer number and a lead's cards
/// by its external reference, so both lookups need an index.
/// </summary>
[TestFixture]
public class CrmLookupIndexesSchemaTests
{
    // A model-only context: an explicit server version, so nothing connects to a database.
    private static KanbanPnDbContext Context() => new(new DbContextOptionsBuilder<KanbanPnDbContext>()
        .UseMySql("Server=localhost;Database=kanban-model-only;user=root;password=unused",
            new MariaDbServerVersion(new Version(10, 8)))
        .Options);

    [Test]
    public void CardTenant_HasIndexLedByCustomerNo()
    {
        using var ctx = Context();
        Assert.That(ctx.Model.FindEntityType(typeof(CardTenant))!.GetIndexes()
            .Any(i => i.Properties.Select(p => p.Name).SequenceEqual(new[] { "CustomerNo" }) && !i.IsUnique), Is.True);
    }

    [Test]
    public void Card_HasIndexOnExternalSourceAndExternalId()
    {
        using var ctx = Context();
        Assert.That(ctx.Model.FindEntityType(typeof(Card))!.GetIndexes()
            .Any(i => i.Properties.Select(p => p.Name).SequenceEqual(new[] { "ExternalSource", "ExternalId" }) && !i.IsUnique), Is.True);
    }

    [Test]
    public void Migrations_IncludeCrmLookupIndexes_AndMatchTheModel()
    {
        using var ctx = Context();
        Assert.That(ctx.Database.GetMigrations(), Has.Some.EndsWith("_CrmLookupIndexes"));
        Assert.That(ctx.Database.HasPendingModelChanges(), Is.False);
    }
}
