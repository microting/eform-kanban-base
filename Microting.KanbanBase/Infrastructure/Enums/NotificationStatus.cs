namespace Microting.KanbanBase.Infrastructure.Enums;

/// <summary>An outbox row's state. Pending rows are due at NextAttemptAt; Sending rows are claimed until LockedUntil.
/// Merged rows were folded into another push; Skipped rows had nothing to deliver to (no device, no address).</summary>
public enum NotificationStatus
{
    Pending = 0,
    Sending = 1,
    Sent = 2,
    Failed = 3,
    Merged = 4,
    Skipped = 5,
}
