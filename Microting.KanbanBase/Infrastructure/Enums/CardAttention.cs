namespace Microting.KanbanBase.Infrastructure.Enums;

/// <summary>
/// What on a card waits for the board owner, set by the implement agent and cleared by it or by hand (spec 2026-10-29).
/// Cards.Attention is an int column: new members are APPENDED, never renumbered.
/// </summary>
public enum CardAttention
{
    None = 0,
    AgentStuck = 1,
    PrAwaitsOwner = 2,
}
