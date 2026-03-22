using Microting.eFormApi.BasePn.Infrastructure.Database.Base;
using Microting.KanbanBase.Infrastructure.Enums;

namespace Microting.KanbanBase.Infrastructure.Data.Entities;

public class BoardMemberVersion : BaseEntity
{
    public int BoardMemberId { get; set; }
    public int BoardId { get; set; }
    public int UserId { get; set; }
    public BoardRole Role { get; set; }
}
