using System;

namespace Microting.KanbanBase.Infrastructure.Data.Entities;

/// <summary>
/// Incremental-sync watermark, stored per Userback project.
/// </summary>
public class UserbackProjectSyncState : KanbanPnBase
{
    public int UserbackProjectId { get; set; }
    public DateTime LastSyncedModifiedAt { get; set; }
}
