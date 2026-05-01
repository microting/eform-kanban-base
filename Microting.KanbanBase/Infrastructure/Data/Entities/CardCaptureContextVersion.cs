using System;
using Microting.eFormApi.BasePn.Infrastructure.Database.Base;

namespace Microting.KanbanBase.Infrastructure.Data.Entities;

public class CardCaptureContextVersion : BaseEntity
{
    public int CardCaptureContextId { get; set; }
    public int CardId { get; set; }
    public string? PageUrl { get; set; }
    public string? UserAgent { get; set; }
    public string? BrowserName { get; set; }
    public string? BrowserVersion { get; set; }
    public string? OsName { get; set; }
    public string? OsVersion { get; set; }
    public string? DeviceType { get; set; }
    public string? WindowSize { get; set; }
    public string? ScreenResolution { get; set; }
    public int? Dpi { get; set; }
    public int? ColourDepth { get; set; }
    public DateTime? CapturedAtUtc { get; set; }
    public string? CustomDataJson { get; set; }
    public string? UserDataJson { get; set; }
}
