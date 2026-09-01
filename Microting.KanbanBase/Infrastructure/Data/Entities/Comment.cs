namespace Microting.KanbanBase.Infrastructure.Data.Entities;

public class Comment : KanbanPnBase
{
    public int CardId { get; set; }
    public virtual Card Card { get; set; }
    public int AuthorUserId { get; set; }
    public string Body { get; set; } = string.Empty;

    /// <summary>
    /// Source id of the upstream Userback comment this row was imported from; NULL for
    /// manually-authored comments. Carries a UNIQUE index so a re-sync cannot duplicate an
    /// upstream comment. This is intentional and safe: InnoDB permits any number of NULLs in
    /// a unique index, so user-authored comments (which leave it NULL) are unaffected.
    /// Do not "fix" the index by removing the uniqueness.
    /// </summary>
    public long? UserbackCommentId { get; set; }
}
