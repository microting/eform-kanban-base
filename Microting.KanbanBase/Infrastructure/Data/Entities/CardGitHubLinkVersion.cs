using Microting.eFormApi.BasePn.Infrastructure.Database.Base;

namespace Microting.KanbanBase.Infrastructure.Data.Entities;

public class CardGitHubLinkVersion : BaseEntity
{
    public int CardGitHubLinkId { get; set; }
    public int CardId { get; set; }
    public int ProjectRepositoryId { get; set; }
    public int IssueNumber { get; set; }
}
