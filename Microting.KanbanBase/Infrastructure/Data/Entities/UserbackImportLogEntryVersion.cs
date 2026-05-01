using Microting.eFormApi.BasePn.Infrastructure.Database.Base;
using Microting.KanbanBase.Infrastructure.Enums;

namespace Microting.KanbanBase.Infrastructure.Data.Entities;

public class UserbackImportLogEntryVersion : BaseEntity
{
    public int UserbackImportLogEntryId { get; set; }
    public int RunId { get; set; }
    public long UserbackFeedbackId { get; set; }
    public int? CardId { get; set; }
    public UserbackImportLogEntryStatus Status { get; set; }
    public string? ErrorMessage { get; set; }
}
