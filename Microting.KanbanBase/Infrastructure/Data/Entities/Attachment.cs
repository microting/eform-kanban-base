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

    /// <summary>
    /// Upstream URL the file was fetched from (Userback CDN); NULL for locally uploaded files.
    /// Intentionally unbounded (longtext): the signed share-viewer URLs this holds routinely
    /// exceed any workable varchar cap, and under STRICT_TRANS_TABLES an over-length value
    /// throws instead of truncating. Index and dedupe on <see cref="SourceUrlHash"/>, never on
    /// this column.
    /// </summary>
    public string? SourceUrl { get; set; }

    /// <summary>
    /// SHA-256 hex digest (64 chars, lowercase) of <see cref="SourceUrl"/>; NULL when SourceUrl
    /// is NULL. This is the indexable dedupe key for imported media — a fixed-width stand-in for
    /// a column too long to index, since utf8mb4 varchar(2048) alone would blow past InnoDB's
    /// 3072-byte index limit. A re-sync matches on (CardId, SourceUrlHash) so it does not
    /// re-download and re-insert every attachment.
    /// </summary>
    public string? SourceUrlHash { get; set; }
}
