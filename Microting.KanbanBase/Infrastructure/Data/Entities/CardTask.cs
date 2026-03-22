using System;
using Microting.KanbanBase.Infrastructure.Enums;

namespace Microting.KanbanBase.Infrastructure.Data.Entities;

public class CardTask : KanbanPnBase
{
    public int CardId { get; set; }
    public virtual Card Card { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public CardType TaskType { get; set; }
    public Priority Priority { get; set; }
    public string? Estimation { get; set; }
    public DateTime? DueDate { get; set; }
    public TimeOnly? DueTime { get; set; }
    public ReminderType Reminder { get; set; }
    public int? AssignedToUserId { get; set; }
    public bool IsCompleted { get; set; }
    public int Position { get; set; }
}
