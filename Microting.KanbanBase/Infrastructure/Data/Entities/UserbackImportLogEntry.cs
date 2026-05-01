using Microting.KanbanBase.Infrastructure.Enums;

namespace Microting.KanbanBase.Infrastructure.Data.Entities;

public class UserbackImportLogEntry : KanbanPnBase
{
    public int RunId { get; set; }
    public virtual UserbackImportRun Run { get; set; }
    public long UserbackFeedbackId { get; set; }
    public int? CardId { get; set; }
    public virtual Card? Card { get; set; }
    public UserbackImportLogEntryStatus Status { get; set; }
    public string? ErrorMessage { get; set; }
}
