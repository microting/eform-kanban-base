#nullable enable
using System.Reflection;
using Microting.KanbanBase.Infrastructure.Data.Entities;
using Microting.KanbanBase.Infrastructure.Enums;
using NUnit.Framework;

namespace Microting.KanbanBase.Tests;

[TestFixture]
public class UserbackMigrationTests
{
    [Test]
    public void Card_NewFields_HaveCorrectDefaults()
    {
        var card = new Card();

        Assert.That(card.FeedbackType, Is.EqualTo(FeedbackType.None));
        Assert.That(card.Source, Is.EqualTo(CardSource.Manual));
        Assert.That(card.VoteCount, Is.EqualTo(0));
        Assert.That(card.Rating, Is.Null);
        Assert.That(card.UserbackFeedbackId, Is.Null);
        Assert.That(card.SubmitterName, Is.Null);
        Assert.That(card.SubmitterEmail, Is.Null);
        Assert.That(card.CaptureContext, Is.Null);
        Assert.That(card.ConsoleLogs, Is.Not.Null);
        Assert.That(card.ConsoleLogs, Is.Empty);
    }

    [Test]
    public void CardCaptureContext_CanBeConstructed()
    {
        var ctx = new CardCaptureContext
        {
            CardId = 42,
            PageUrl = "https://example.com/dashboard",
            Dpi = 192
        };

        Assert.That(ctx.CardId, Is.EqualTo(42));
        Assert.That(ctx.PageUrl, Is.EqualTo("https://example.com/dashboard"));
        Assert.That(ctx.Dpi, Is.EqualTo(192));
    }

    [Test]
    public void CardConsoleLog_DefaultLevelIsLog()
    {
        var entry = new CardConsoleLog();

        Assert.That(entry.Level, Is.EqualTo(ConsoleLogLevel.Log));
        Assert.That(entry.Message, Is.EqualTo(string.Empty));
    }

    [Test]
    public void UserbackImportRun_DefaultStatusIsPending()
    {
        var run = new UserbackImportRun();

        Assert.That(run.Status, Is.EqualTo(UserbackImportRunStatus.Pending));
        Assert.That(run.Entries, Is.Not.Null);
        Assert.That(run.Entries, Is.Empty);
        Assert.That(run.ProjectsJson, Is.EqualTo(string.Empty));
    }

    [Test]
    public void UserbackImportLogEntry_CardIdIsNullable()
    {
        PropertyInfo? cardIdProp = typeof(UserbackImportLogEntry).GetProperty(nameof(UserbackImportLogEntry.CardId));

        Assert.That(cardIdProp, Is.Not.Null);
        Assert.That(cardIdProp!.PropertyType, Is.EqualTo(typeof(int?)));
    }
}
