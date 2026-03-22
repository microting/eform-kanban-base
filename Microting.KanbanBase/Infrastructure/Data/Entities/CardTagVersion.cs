using Microting.eFormApi.BasePn.Infrastructure.Database.Base;

namespace Microting.KanbanBase.Infrastructure.Data.Entities;

public class CardTagVersion : BaseEntity
{
    public int CardTagId { get; set; }
    public int CardId { get; set; }
    public int TagId { get; set; }
}
