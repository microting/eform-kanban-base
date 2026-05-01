using System;
using System.Collections.Generic;
using Microting.KanbanBase.Infrastructure.Enums;

namespace Microting.KanbanBase.Infrastructure.Data.Entities;

public class UserbackImportRun : KanbanPnBase
{
    public DateTime StartedAt { get; set; }
    public DateTime? FinishedAt { get; set; }
    public int RunByUserId { get; set; }
    public string ProjectsJson { get; set; } = string.Empty;
    public UserbackImportRunStatus Status { get; set; } = UserbackImportRunStatus.Pending;
    public string? ErrorMessage { get; set; }
    public int CardsImported { get; set; }
    public int AttachmentsImported { get; set; }
    public int CommentsImported { get; set; }
    public virtual ICollection<UserbackImportLogEntry> Entries { get; set; } = new List<UserbackImportLogEntry>();
}
