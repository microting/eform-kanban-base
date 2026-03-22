using Microting.eFormApi.BasePn.Infrastructure.Database.Base;
using Microting.KanbanBase.Infrastructure.Enums;

namespace Microting.KanbanBase.Infrastructure.Data.Entities;

public class ActivityLogEntryVersion : BaseEntity
{
    public int ActivityLogEntryId { get; set; }
    public int CardId { get; set; }
    public int BoardId { get; set; }
    public int UserId { get; set; }
    public ActivityAction Action { get; set; }
    public string? OldValue { get; set; }
    public string? NewValue { get; set; }
}
