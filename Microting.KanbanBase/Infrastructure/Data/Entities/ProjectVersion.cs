using Microting.eFormApi.BasePn.Infrastructure.Database.Base;

namespace Microting.KanbanBase.Infrastructure.Data.Entities;

public class ProjectVersion : BaseEntity
{
    public int ProjectId { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public string? GitHubOwner { get; set; }
    public string? GitHubRepo { get; set; }
    public string? GitHubPatEncrypted { get; set; }
    public string? GitHubPatIv { get; set; }
}
