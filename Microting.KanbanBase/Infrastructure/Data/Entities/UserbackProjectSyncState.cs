using System;

namespace Microting.KanbanBase.Infrastructure.Data.Entities;

/// <summary>
/// Incremental-sync watermark, stored per Userback project.
/// </summary>
public class UserbackProjectSyncState : KanbanPnBase
{
    public int UserbackProjectId { get; set; }

    /// <summary>
    /// High-water mark of the newest upstream ModifiedAt already pulled for this project.
    /// NULL means "never synced" — do a full pull. Deliberately nullable rather than relying on
    /// default(DateTime): 0001-01-01 is outside MySQL/MariaDB's DATETIME range and the server
    /// runs STRICT_TRANS_TABLES, so such an insert throws rather than coercing.
    /// </summary>
    public DateTime? LastSyncedModifiedAt { get; set; }
}
