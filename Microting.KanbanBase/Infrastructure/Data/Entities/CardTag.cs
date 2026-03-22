namespace Microting.KanbanBase.Infrastructure.Data.Entities;

public class CardTag : KanbanPnBase
{
    public int CardId { get; set; }
    public virtual Card Card { get; set; }
    public int TagId { get; set; }
    public virtual Tag Tag { get; set; }
}
