namespace Microting.KanbanBase.Infrastructure.Enums;

/// <summary>What happened (spec 2026-10-30 §5). Persisted as int: append only. AnswersPosted is the webhook's event and
/// never a user preference.</summary>
public enum NotificationEvent
{
    AgentQuestion = 1,
    NeedsYou = 2,
    Mention = 3,
    Reply = 4,
    StatusChange = 5,
    AnswersPosted = 6,
}
