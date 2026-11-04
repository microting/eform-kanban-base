namespace Microting.KanbanBase.Infrastructure.Data.Entities;

/// <summary>The answer comment a person wrote through the answers endpoint, and the question set (the questions task) it
/// answers (spec 2026-10-30 §7). No foreign keys: comments are hard-deleted, and a link to a deleted comment is ignored.</summary>
public class AnswerLink : KanbanPnBase
{
    public int CardId { get; set; }
    /// <summary>The question set: the "Besvar spørgsmål" task's id.</summary>
    public int TaskId { get; set; }
    public int CommentId { get; set; }
    public int AnsweredByUserId { get; set; }
}
