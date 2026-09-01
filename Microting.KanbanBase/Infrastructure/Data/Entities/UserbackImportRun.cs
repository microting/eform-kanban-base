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
    public UserbackImportMode Mode { get; set; } = UserbackImportMode.Incremental;
    public string? ErrorMessage { get; set; }

    // Legacy aggregate counter — kept for backwards compatibility, still read by existing code.
    public int CardsImported { get; set; }

    // Split counters
    public int CardsCreated { get; set; }
    public int CardsUpdated { get; set; }
    public int CardsSkipped { get; set; }
    public int CardsSoftDeleted { get; set; }
    public int MediaFailed { get; set; }

    // Preview-independent total, so progress can be shown without a preview step.
    public int TotalToProcess { get; set; }

    public int AttachmentsImported { get; set; }
    public int CommentsImported { get; set; }

    // Liveness marker — lets a startup sweep age out runs orphaned by a pod recycle.
    public DateTime? LastHeartbeatAt { get; set; }

    public virtual ICollection<UserbackImportLogEntry> Entries { get; set; } = new List<UserbackImportLogEntry>();
}
