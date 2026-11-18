#nullable enable
using Microting.KanbanBase.Infrastructure.Enums;
using NUnit.Framework;

namespace Microting.KanbanBase.Tests;

/// <summary>
/// CardType is stored as an int and sent as an int over the integration API, so every value keeps its number.
/// Support (5) lets the CRM tell support cards from development cards on one board.
/// </summary>
[TestFixture]
public class CardTypeValuesTests
{
    [TestCase(CardType.Bug, 0)]
    [TestCase(CardType.Feature, 1)]
    [TestCase(CardType.Task, 2)]
    [TestCase(CardType.Story, 3)]
    [TestCase(CardType.Epic, 4)]
    [TestCase(CardType.Support, 5)]
    public void CardType_KeepsItsStoredNumber(CardType type, int stored)
    {
        Assert.That((int)type, Is.EqualTo(stored));
    }
}
