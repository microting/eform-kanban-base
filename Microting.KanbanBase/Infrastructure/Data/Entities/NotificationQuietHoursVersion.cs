using Microting.eFormApi.BasePn.Infrastructure.Database.Base;

namespace Microting.KanbanBase.Infrastructure.Data.Entities;

public class NotificationQuietHoursVersion : BaseEntity
{
    public int NotificationQuietHoursId { get; set; }
    public int UserId { get; set; }
    public bool Enabled { get; set; }
    public int FromMinute { get; set; }
    public int ToMinute { get; set; }
    public string TimeZone { get; set; } = string.Empty;
}
