#nullable enable
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microting.KanbanBase.Infrastructure.Data.Entities;
using Microting.KanbanBase.Infrastructure.Enums;
using NUnit.Framework;

namespace Microting.KanbanBase.Tests;

/// <summary>
/// Spec 2026-10-29 §4 ("Venter på dig"): Card.Attention, AttentionNote, AttentionChangedAt, the AttentionChanged
/// activity action and the migration that adds them.
/// </summary>
[TestFixture]
public class CardAttentionSchemaTests
{
    private const string MigrationId = "20261029000000_CardAttention";

    [Test]
    public void Card_Attention_DefaultsToNone()
    {
        var card = new Card();
        Assert.That(card.Attention, Is.EqualTo(CardAttention.None));
        Assert.That(card.AttentionNote, Is.Null);
        Assert.That(card.AttentionChangedAt, Is.Null);
    }

    // Cards.Attention is an int column: renumbering a member reinterprets every existing row.
    [Test]
    public void CardAttention_PersistedValues()
    {
        Assert.Multiple(() =>
        {
            Assert.That((int)CardAttention.None, Is.EqualTo(0));
            Assert.That((int)CardAttention.AgentStuck, Is.EqualTo(1));
            Assert.That((int)CardAttention.PrAwaitsOwner, Is.EqualTo(2));
        });
    }

    [Test]
    public void ActivityAction_AttentionChangedIsAppended()
    {
        Assert.That((int)ActivityAction.DependencyRemoved, Is.EqualTo(14));
        Assert.That((int)ActivityAction.AttentionChanged, Is.EqualTo(15));
        var values = Enum.GetValues<ActivityAction>().Cast<int>().OrderBy(v => v).ToArray();
        Assert.That(values, Is.EqualTo(Enumerable.Range(0, values.Length).ToArray()));
    }

    // KanbanPnBase.MapVersion copies by name and only logs a miss to the console.
    [Test]
    public void CardVersion_CarriesTheAttentionFields()
    {
        foreach (var name in new[] { nameof(Card.Attention), nameof(Card.AttentionNote), nameof(Card.AttentionChangedAt) })
        {
            Assert.That(typeof(CardVersion).GetProperty(name)?.PropertyType,
                Is.EqualTo(typeof(Card).GetProperty(name)!.PropertyType), name);
        }
    }

    [Test]
    public void Migration_AddsTheColumnsToCardsAndCardVersions()
    {
        var type = typeof(Card).Assembly.GetTypes().Single(t =>
            t.GetCustomAttributes(typeof(MigrationAttribute), false).Cast<MigrationAttribute>().Any(a => a.Id == MigrationId));
        var migration = (Migration)Activator.CreateInstance(type)!;
        var added = migration.UpOperations.OfType<AddColumnOperation>().ToList();

        Assert.That(added.Select(o => (o.Table, o.Name)), Is.SupersetOf(new[]
        {
            ("Cards", "Attention"), ("Cards", "AttentionNote"), ("Cards", "AttentionChangedAt"),
            ("CardVersions", "Attention"), ("CardVersions", "AttentionNote"), ("CardVersions", "AttentionChangedAt"),
        }));
        var attention = added.Single(o => o.Table == "Cards" && o.Name == "Attention");
        Assert.That(attention.IsNullable, Is.False);
        Assert.That(attention.DefaultValue, Is.EqualTo(0), "existing cards wait for nothing");
        Assert.That(added.Single(o => o.Table == "Cards" && o.Name == "AttentionNote").MaxLength, Is.EqualTo(200));
    }

    [Test]
    public void Snapshot_KnowsTheNewColumns()
    {
        var type = typeof(Card).Assembly.GetType("Microting.KanbanBase.Migrations.KanbanPnDbContextModelSnapshot", throwOnError: true)!;
        var snapshot = (ModelSnapshot)Activator.CreateInstance(type, nonPublic: true)!;
        var card = snapshot.Model.FindEntityType(typeof(Card).FullName!)!;

        Assert.That(card.FindProperty(nameof(Card.Attention)), Is.Not.Null);
        Assert.That(card.FindProperty(nameof(Card.AttentionNote))!.GetMaxLength(), Is.EqualTo(200));
        Assert.That(card.FindProperty(nameof(Card.AttentionChangedAt)), Is.Not.Null);
    }
}
