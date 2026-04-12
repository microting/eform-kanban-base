using Microting.eFormApi.BasePn.Infrastructure.Database.Base;
using Microting.KanbanBase.Infrastructure.Enums;

namespace Microting.KanbanBase.Infrastructure.Data.Entities;

public class ProjectRepositoryVersion : BaseEntity
{
    public int ProjectRepositoryId { get; set; }
    public int ProjectId { get; set; }
    public string Owner { get; set; } = string.Empty;
    public string Repo { get; set; } = string.Empty;
    public RepositorySource Source { get; set; }
    public string? GitHubPatEncrypted { get; set; }
    public string? GitHubPatIv { get; set; }
}
