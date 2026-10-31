using System;
using Microting.eFormApi.BasePn.Infrastructure.Database.Base;
using Microting.KanbanBase.Infrastructure.Enums;

namespace Microting.KanbanBase.Infrastructure.Data.Entities;

public class WorkspaceMemberVersion : BaseEntity
{
    public int WorkspaceMemberId { get; set; }
    public int WorkspaceId { get; set; }
    public int UserId { get; set; }
    public WorkspaceRole Role { get; set; }
    public int? CustomerId { get; set; }
    public int? InvitedByUserId { get; set; }
    public DateTime JoinedAt { get; set; }
}
