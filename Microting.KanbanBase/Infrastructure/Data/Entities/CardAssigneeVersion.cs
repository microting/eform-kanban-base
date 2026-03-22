using Microting.eFormApi.BasePn.Infrastructure.Database.Base;

namespace Microting.KanbanBase.Infrastructure.Data.Entities;

public class CardAssigneeVersion : BaseEntity
{
    public int CardAssigneeId { get; set; }
    public int CardId { get; set; }
    public int UserId { get; set; }
}
