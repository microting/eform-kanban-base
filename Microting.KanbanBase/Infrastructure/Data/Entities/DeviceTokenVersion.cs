using System;
using Microting.eFormApi.BasePn.Infrastructure.Database.Base;

namespace Microting.KanbanBase.Infrastructure.Data.Entities;

public class DeviceTokenVersion : BaseEntity
{
    public int DeviceTokenId { get; set; }
    public int UserId { get; set; }
    public string AppId { get; set; } = string.Empty;
    public string InstallationId { get; set; } = string.Empty;
    public string Platform { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
    public string? AppVersion { get; set; }
    public DateTime LastSeenAt { get; set; }
}
