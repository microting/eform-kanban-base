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
}
