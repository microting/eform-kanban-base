namespace Microting.KanbanBase.Infrastructure.Data.Entities;

/// <summary>One user's quiet hours, in every workspace: pushes inside [From, To) local time are held to the end.
/// Minutes after local midnight; From &gt; To crosses midnight.</summary>
public class NotificationQuietHours : KanbanPnBase
{
    public int UserId { get; set; }
    public bool Enabled { get; set; }
    public int FromMinute { get; set; } = 22 * 60;
    public int ToMinute { get; set; } = 7 * 60;
    /// <summary>An IANA zone id, e.g. Europe/Copenhagen.</summary>
    public string TimeZone { get; set; } = "Europe/Copenhagen";
}
