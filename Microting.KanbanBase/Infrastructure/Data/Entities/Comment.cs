namespace Microting.KanbanBase.Infrastructure.Data.Entities;

public class Comment : KanbanPnBase
{
    public int CardId { get; set; }
    public virtual Card Card { get; set; }
    public int AuthorUserId { get; set; }
    public string Body { get; set; } = string.Empty;

    /// <summary>
    /// Source id of the upstream Userback comment this row was imported from; NULL for
    /// manually-authored comments.
    /// <para>
    /// The backing index (CardId, UserbackCommentId) is deliberately NON-unique, matching
    /// corrections C5/C6 on the tracking issue. <see cref="KanbanPnBase.Delete"/> SOFT-deletes,
    /// so a removed row keeps both its UserbackCommentId and its index slot, and MySQL has no
    /// filtered indexes — a unique index would therefore permanently block re-importing a
    /// comment that had once been deleted. Re-import uniqueness is enforced in code instead:
    /// look the row up with FirstOrDefaultAsync on (CardId, UserbackCommentId) and only insert
    /// when nothing comes back. Do not add IsUnique() here.
    /// </para>
    /// </summary>
    public long? UserbackCommentId { get; set; }

    /// <summary>Where an integration comment came from outside kanban ("gmail" for a support-mail reply); null for
    /// every other comment. Set when the comment is written and never changed.</summary>
    public string? ExternalSource { get; set; }
}
