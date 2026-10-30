#nullable enable
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microting.KanbanBase.Infrastructure.Data;
using Microting.KanbanBase.Infrastructure.Data.Entities;
using NUnit.Framework;

namespace Microting.KanbanBase.Tests;

/// <summary>
/// The CommentExternalSource migration: where an integration comment came from outside kanban ("gmail" for a
/// support-mail reply), so mail provenance lives on the comment and not on who wrote it.
/// </summary>
[TestFixture]
public class CommentExternalSourceSchemaTests
{
    // A model-only context: an explicit server version, so nothing connects to a database.
    private static KanbanPnDbContext Context() => new(new DbContextOptionsBuilder<KanbanPnDbContext>()
        .UseMySql("Server=localhost;Database=kanban-model-only;user=root;password=unused",
            new MariaDbServerVersion(new Version(10, 8)))
        .Options);

    [Test]
    public void Comment_ExternalSource_DefaultsToNull_AndRoundTrips()
    {
        var comment = new Comment();
        Assert.That(comment.ExternalSource, Is.Null);
        comment.ExternalSource = "gmail";
        Assert.That(comment.ExternalSource, Is.EqualTo("gmail"));
    }

    [Test]
    public void CommentVersion_HasExternalSource()
    {
        // KanbanPnBase.MapVersion copies by name and only logs a miss: the version twin must have it.
        Assert.That(typeof(CommentVersion).GetProperty(nameof(Comment.ExternalSource))?.PropertyType,
            Is.EqualTo(typeof(string)));
    }

    [Test]
    public void Model_ExternalSource_IsNullableWithMaxLength32()
    {
        using var ctx = Context();
        var property = ctx.Model.FindEntityType(typeof(Comment))!.FindProperty(nameof(Comment.ExternalSource))!;
        Assert.Multiple(() =>
        {
            Assert.That(property.IsNullable, Is.True);
            Assert.That(property.GetMaxLength(), Is.EqualTo(32));
        });
    }

    [Test]
    public void Migrations_IncludeCommentExternalSource_AndMatchTheModel()
    {
        using var ctx = Context();
        Assert.That(ctx.Database.GetMigrations(), Has.Some.EndsWith("_CommentExternalSource"));
        Assert.That(ctx.Database.HasPendingModelChanges(), Is.False);
    }
}
