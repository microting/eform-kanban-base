namespace Microting.KanbanBase.Infrastructure.Data.Entities;

public class Comment : KanbanPnBase
{
    public int CardId { get; set; }
    public virtual Card Card { get; set; }
    public int AuthorUserId { get; set; }
    public string Body { get; set; } = string.Empty;
}
