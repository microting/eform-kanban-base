using System;
using Microting.eFormApi.BasePn.Infrastructure.Database.Base;
using Microting.KanbanBase.Infrastructure.Enums;

namespace Microting.KanbanBase.Infrastructure.Data.Entities;

public class CardVersion : BaseEntity
{
    public int CardId { get; set; }
    public int BoardId { get; set; }
    public int ColumnId { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public int Position { get; set; }
    public CardType CardType { get; set; }
    public Priority Priority { get; set; }
    public int? StoryPoints { get; set; }
    public DateTime? DueDate { get; set; }
    public int? SprintId { get; set; }
    public int? ProjectId { get; set; }
    public int? GitHubIssueNumber { get; set; }
}
