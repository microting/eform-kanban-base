using Microting.KanbanBase.Infrastructure.Enums;

namespace Microting.KanbanBase.Infrastructure.Data.Entities;

public class BoardMember : KanbanPnBase
{
    public int BoardId { get; set; }
    public virtual Board Board { get; set; }
    public int UserId { get; set; }
    public BoardRole Role { get; set; }
}
