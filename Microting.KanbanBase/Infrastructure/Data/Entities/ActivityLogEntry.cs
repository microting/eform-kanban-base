using Microting.KanbanBase.Infrastructure.Enums;

namespace Microting.KanbanBase.Infrastructure.Data.Entities;

public class ActivityLogEntry : KanbanPnBase
{
    public int CardId { get; set; }
    public virtual Card Card { get; set; }
    public int BoardId { get; set; }
    public int UserId { get; set; }
    public ActivityAction Action { get; set; }
    public string? OldValue { get; set; }
    public string? NewValue { get; set; }
}
