using System;
using Microting.KanbanBase.Infrastructure.Enums;

namespace Microting.KanbanBase.Infrastructure.Data.Entities;

/// <summary>
/// One delivery: a push or an email to one user, or a webhook POST. Written by Kanban.Pn's NotificationOutboxWriter,
/// claimed and finished by its NotificationDeliverer with conditional UPDATEs. A queue, not a document: rows are written
/// and changed without Version rows (the twin exists for the base convention).
/// </summary>
public class NotificationOutbox : KanbanPnBase
{
    public int WorkspaceId { get; set; }
    public int? BoardId { get; set; }
    public int? CardId { get; set; }
    public NotificationEvent Event { get; set; }
    public NotificationChannel Channel { get; set; }
    /// <summary>Null for a webhook row.</summary>
    public int? RecipientUserId { get; set; }
    public int? ActorUserId { get; set; }
    /// <summary>How many events this row stands for (merged pushes add up; "3 spørgsmål venter på dig").</summary>
    public int Count { get; set; } = 1;
    /// <summary>JSON the deliverer needs (email snapshot, push variant, webhook body). Never logged.</summary>
    public string? Payload { get; set; }
    public NotificationStatus Status { get; set; } = NotificationStatus.Pending;
    public int Attempts { get; set; }
    public DateTime NextAttemptAt { get; set; }
    public DateTime? LockedUntil { get; set; }
    /// <summary>Held to the end of the recipient's quiet hours; due held pushes are summarised into one.</summary>
    public bool HeldForQuietHours { get; set; }
    public DateTime? SentAt { get; set; }
    public string? LastError { get; set; }
}
