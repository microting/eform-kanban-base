namespace Microting.KanbanBase.Infrastructure.Data.Entities;

/// <summary>A user follows a card (explicitly, or by creating, commenting on or answering it). Assignees follow without a
/// row. Unique (CardId, UserId) without WorkflowState: unfollow soft-deletes, follow revives.</summary>
public class CardFollower : KanbanPnBase
{
    public int CardId { get; set; }
    public virtual Card Card { get; set; } = null!;
    public int UserId { get; set; }
}
