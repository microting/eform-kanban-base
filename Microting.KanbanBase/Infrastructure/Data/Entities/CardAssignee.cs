namespace Microting.KanbanBase.Infrastructure.Data.Entities;

public class CardAssignee : KanbanPnBase
{
    public int CardId { get; set; }
    public virtual Card Card { get; set; }
    public int UserId { get; set; }
}
