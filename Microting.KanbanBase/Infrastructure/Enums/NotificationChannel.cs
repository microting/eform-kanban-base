namespace Microting.KanbanBase.Infrastructure.Enums;

/// <summary>How an outbox row is delivered. Persisted as int: append only.</summary>
public enum NotificationChannel
{
    Push = 1,
    Email = 2,
    Webhook = 3,
}
