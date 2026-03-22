using Microting.eFormApi.BasePn.Infrastructure.Database.Base;

namespace Microting.KanbanBase.Infrastructure.Data.Entities;

public class CommentVersion : BaseEntity
{
    public int CommentId { get; set; }
    public int CardId { get; set; }
    public int AuthorUserId { get; set; }
    public string Body { get; set; }
}
