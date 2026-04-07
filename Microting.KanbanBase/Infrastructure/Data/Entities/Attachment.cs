namespace Microting.KanbanBase.Infrastructure.Data.Entities;

public class Attachment : KanbanPnBase
{
    public int CardId { get; set; }
    public virtual Card Card { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string StorageFileName { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public int UploadedByUserId { get; set; }

    // Device metadata from Chrome extension
    public string? Source { get; set; }
    public string? PageUrl { get; set; }
    public string? SystemInfo { get; set; }
    public string? BrowserInfo { get; set; }
    public int? ScreenWidth { get; set; }
    public int? ScreenHeight { get; set; }
    public int? BrowserWindowWidth { get; set; }
    public int? BrowserWindowHeight { get; set; }
    public double? DevicePixelRatio { get; set; }
    public int? ColorDepth { get; set; }
    public string? Location { get; set; }
    public int? VideoDurationMs { get; set; }
}
