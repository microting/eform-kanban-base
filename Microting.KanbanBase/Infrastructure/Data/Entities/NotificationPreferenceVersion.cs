using Microting.eFormApi.BasePn.Infrastructure.Database.Base;
using Microting.KanbanBase.Infrastructure.Enums;

namespace Microting.KanbanBase.Infrastructure.Data.Entities;

public class NotificationPreferenceVersion : BaseEntity
{
    public int NotificationPreferenceId { get; set; }
    public int UserId { get; set; }
    public int WorkspaceId { get; set; }
    public NotificationEvent Event { get; set; }
    public bool Push { get; set; }
    public bool Email { get; set; }
    public bool AllCards { get; set; }
}
