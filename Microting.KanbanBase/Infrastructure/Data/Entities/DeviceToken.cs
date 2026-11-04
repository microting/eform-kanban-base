using System;

namespace Microting.KanbanBase.Infrastructure.Data.Entities;

/// <summary>A phone app installation of a user and its current FCM token. The identity is (AppId, InstallationId), not the
/// token: tokens rotate and an installation can be logged into by another user. Unique index without WorkflowState,
/// so registration revives a removed row.</summary>
public class DeviceToken : KanbanPnBase
{
    public int UserId { get; set; }
    public string AppId { get; set; } = string.Empty;
    public string InstallationId { get; set; } = string.Empty;
    /// <summary>"android" or "ios".</summary>
    public string Platform { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
    public string? AppVersion { get; set; }
    public DateTime LastSeenAt { get; set; }
}
