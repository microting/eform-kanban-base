namespace Microting.KanbanBase.Infrastructure.Data.Entities;

/// <summary>An end customer of a workspace. Cards link to it through CardTenant.CustomerId.</summary>
public class Customer : KanbanPnBase
{
    public int WorkspaceId { get; set; }
    public virtual Workspace Workspace { get; set; } = null!;
    public string Name { get; set; } = string.Empty;
    /// <summary>The customer number in the workspace's own systems (workspace 1: the eForm customer number); optional.</summary>
    public string? CustomerNo { get; set; }
}
