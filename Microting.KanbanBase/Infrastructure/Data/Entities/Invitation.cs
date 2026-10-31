using System;
using Microting.KanbanBase.Infrastructure.Enums;

namespace Microting.KanbanBase.Infrastructure.Data.Entities;

/// <summary>An invitation to join a workspace. Only the SHA-256 hex of the link token is stored.</summary>
public class Invitation : KanbanPnBase
{
    public int WorkspaceId { get; set; }
    public virtual Workspace Workspace { get; set; } = null!;
    public string Email { get; set; } = string.Empty;
    public WorkspaceRole Role { get; set; }
    /// <summary>Set only when Role is Customer.</summary>
    public int? CustomerId { get; set; }
    public string TokenHash { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public DateTime? AcceptedAt { get; set; }
    public int? AcceptedByUserId { get; set; }
    public int InvitedByUserId { get; set; }
}
