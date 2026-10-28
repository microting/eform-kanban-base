#nullable enable
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microting.KanbanBase.Infrastructure.Data;
using Microting.KanbanBase.Infrastructure.Data.Entities;
using NUnit.Framework;

namespace Microting.KanbanBase.Tests;

/// <summary>
/// Covers 20261028000000_RestrictCardColumnDelete: a hard delete of a column must never take its
/// cards (and, through their own cascades, every comment, attachment and log row) with it.
/// </summary>
[TestFixture]
public class CardColumnDeleteBehaviorTests
{
    // No connection is opened: building the model and reading the migrations assembly is offline.
    private static KanbanPnDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<KanbanPnDbContext>()
            .UseMySql("Server=localhost;Database=kanban-pn-model-only;user=root;password=unused;",
                new MariaDbServerVersion(new Version(10, 8)))
            .Options;
        return new KanbanPnDbContext(options);
    }

    private static IForeignKey CardColumnForeignKey(IModel model)
    {
        var card = model.FindEntityType(typeof(Card)) ?? model.FindEntityType(typeof(Card).FullName!);
        Assert.That(card, Is.Not.Null);
        return card!.GetForeignKeys().Single(fk => fk.Properties.Single().Name == nameof(Card.ColumnId));
    }

    [Test]
    public void CardColumnForeignKey_IsRestrict_InModel()
    {
        using var context = CreateContext();

        Assert.That(CardColumnForeignKey(context.Model).DeleteBehavior, Is.EqualTo(DeleteBehavior.Restrict));
    }

    [Test]
    public void CardColumnForeignKey_IsRestrict_InMigrationSnapshot()
    {
        using var context = CreateContext();
        var snapshot = context.GetService<IMigrationsAssembly>().ModelSnapshot;

        Assert.That(snapshot, Is.Not.Null);
        Assert.That(CardColumnForeignKey(snapshot!.Model).DeleteBehavior, Is.EqualTo(DeleteBehavior.Restrict));
    }

    [Test]
    public void RestrictCardColumnDeleteMigration_IsRegistered()
    {
        using var context = CreateContext();
        var migrations = context.GetService<IMigrationsAssembly>().Migrations;

        Assert.That(migrations.Keys, Does.Contain("20261028000000_RestrictCardColumnDelete"));
    }
}
