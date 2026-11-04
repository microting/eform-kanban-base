using Microting.KanbanBase.Infrastructure.Enums;

namespace Microting.KanbanBase.Infrastructure.Data.Entities;

/// <summary>One user's switches for one event in one workspace. A missing row means the defaults (push on; email on for
/// AgentQuestion only). AllCards: the event for every card of the workspace, not only assigned or followed ones.</summary>
public class NotificationPreference : KanbanPnBase
{
    public int UserId { get; set; }
    public int WorkspaceId { get; set; }
    public NotificationEvent Event { get; set; }
    public bool Push { get; set; }
    public bool Email { get; set; }
    public bool AllCards { get; set; }
}
