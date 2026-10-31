using System;
using Microting.eFormApi.BasePn.Infrastructure.Database.Base;
using Microting.KanbanBase.Infrastructure.Enums;

namespace Microting.KanbanBase.Infrastructure.Data.Entities;

public class InvitationVersion : BaseEntity
{
    public int InvitationId { get; set; }
    public int WorkspaceId { get; set; }
    public string Email { get; set; } = string.Empty;
    public WorkspaceRole Role { get; set; }
    public int? CustomerId { get; set; }
    public string TokenHash { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public DateTime? AcceptedAt { get; set; }
    public int? AcceptedByUserId { get; set; }
    public int InvitedByUserId { get; set; }
}
