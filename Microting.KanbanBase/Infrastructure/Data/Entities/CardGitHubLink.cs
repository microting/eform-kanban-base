namespace Microting.KanbanBase.Infrastructure.Data.Entities;

public class CardGitHubLink : KanbanPnBase
{
    public int CardId { get; set; }
    public virtual Card Card { get; set; }
    public int ProjectRepositoryId { get; set; }
    public virtual ProjectRepository ProjectRepository { get; set; }
    public int IssueNumber { get; set; }
}
