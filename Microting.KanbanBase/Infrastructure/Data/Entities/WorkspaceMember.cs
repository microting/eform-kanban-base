using System;
using Microting.KanbanBase.Infrastructure.Enums;

namespace Microting.KanbanBase.Infrastructure.Data.Entities;

/// <summary>An eForm user's membership of a workspace. One live row per (WorkspaceId, UserId), enforced in Kanban.Pn.</summary>
public class WorkspaceMember : KanbanPnBase
{
    public int WorkspaceId { get; set; }
    public virtual Workspace Workspace { get; set; } = null!;
    /// <summary>The eForm (host) user id.</summary>
    public int UserId { get; set; }
    public WorkspaceRole Role { get; set; }
    /// <summary>Set only when Role is Customer.</summary>
    public int? CustomerId { get; set; }
    public virtual Customer? Customer { get; set; }
    public int? InvitedByUserId { get; set; }
    public DateTime JoinedAt { get; set; }
}
