using System.Collections.Generic;

namespace Microting.KanbanBase.Infrastructure.Data.Entities;

public class Project : KanbanPnBase
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? GitHubOwner { get; set; }
    public string? GitHubRepo { get; set; }
    public string? GitHubPatEncrypted { get; set; }
    public string? GitHubPatIv { get; set; }

    public virtual ICollection<Card> Cards { get; set; } = new List<Card>();
}
