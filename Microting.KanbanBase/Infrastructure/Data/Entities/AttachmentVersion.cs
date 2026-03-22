using Microting.eFormApi.BasePn.Infrastructure.Database.Base;

namespace Microting.KanbanBase.Infrastructure.Data.Entities;

public class AttachmentVersion : BaseEntity
{
    public int AttachmentId { get; set; }
    public int CardId { get; set; }
    public string FileName { get; set; }
    public string StorageFileName { get; set; }
    public string ContentType { get; set; }
    public long FileSize { get; set; }
    public int UploadedByUserId { get; set; }
}
