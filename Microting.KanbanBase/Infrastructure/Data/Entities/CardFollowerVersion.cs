using Microting.eFormApi.BasePn.Infrastructure.Database.Base;

namespace Microting.KanbanBase.Infrastructure.Data.Entities;

public class CardFollowerVersion : BaseEntity
{
    public int CardFollowerId { get; set; }
    public int CardId { get; set; }
    public int UserId { get; set; }
}
