using Microting.eFormApi.BasePn.Infrastructure.Database.Base;

namespace Microting.KanbanBase.Infrastructure.Data.Entities;

public class AnswerLinkVersion : BaseEntity
{
    public int AnswerLinkId { get; set; }
    public int CardId { get; set; }
    public int TaskId { get; set; }
    public int CommentId { get; set; }
    public int AnsweredByUserId { get; set; }
}
