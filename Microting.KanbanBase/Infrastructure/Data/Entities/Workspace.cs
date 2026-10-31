namespace Microting.KanbanBase.Infrastructure.Data.Entities;

/// <summary>A company using kanban. Workspace 1 is Microting and is seeded.</summary>
public class Workspace : KanbanPnBase
{
    public string Name { get; set; } = string.Empty;
    /// <summary>Unique, lowercase a-z, 0-9 and '-'; never reused, even after the workspace is removed.</summary>
    public string Slug { get; set; } = string.Empty;
}
