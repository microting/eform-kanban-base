using System;
using Microting.eFormApi.BasePn.Infrastructure.Database.Base;
using Microting.KanbanBase.Infrastructure.Enums;

namespace Microting.KanbanBase.Infrastructure.Data.Entities;

public class CardTaskVersion : BaseEntity
{
    public int CardTaskId { get; set; }
    public int CardId { get; set; }
    public string Title { get; set; }
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
