using System.Collections.Generic;

namespace Microting.KanbanBase.Infrastructure.Data.Entities;

public class Project : KanbanPnBase
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public long? GitHubAppInstallationId { get; set; }

    public virtual ICollection<Card> Cards { get; set; } = new List<Card>();
    public virtual ICollection<ProjectRepository> Repositories { get; set; } = new List<ProjectRepository>();
}
