using Microting.KanbanBase.Infrastructure.Enums;

namespace Microting.KanbanBase.Infrastructure.Data.Entities;

public class ProjectRepository : KanbanPnBase
{
    public int ProjectId { get; set; }
    public virtual Project Project { get; set; }
    public string Owner { get; set; } = string.Empty;
    public string Repo { get; set; } = string.Empty;
    public RepositorySource Source { get; set; }
    public string? GitHubPatEncrypted { get; set; }
    public string? GitHubPatIv { get; set; }
}
