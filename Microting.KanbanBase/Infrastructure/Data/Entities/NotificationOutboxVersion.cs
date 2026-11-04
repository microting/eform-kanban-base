using System;
using Microting.eFormApi.BasePn.Infrastructure.Database.Base;
using Microting.KanbanBase.Infrastructure.Enums;

namespace Microting.KanbanBase.Infrastructure.Data.Entities;

public class NotificationOutboxVersion : BaseEntity
{
    public int NotificationOutboxId { get; set; }
    public int WorkspaceId { get; set; }
    public int? BoardId { get; set; }
    public int? CardId { get; set; }
    public NotificationEvent Event { get; set; }
    public NotificationChannel Channel { get; set; }
    public int? RecipientUserId { get; set; }
    public int? ActorUserId { get; set; }
    public int Count { get; set; }
    public string? Payload { get; set; }
    public NotificationStatus Status { get; set; }
    public int Attempts { get; set; }
    public DateTime NextAttemptAt { get; set; }
    public DateTime? LockedUntil { get; set; }
    public bool HeldForQuietHours { get; set; }
    public DateTime? SentAt { get; set; }
    public string? LastError { get; set; }
}
