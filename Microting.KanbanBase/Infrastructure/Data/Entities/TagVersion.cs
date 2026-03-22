using Microting.eFormApi.BasePn.Infrastructure.Database.Base;

namespace Microting.KanbanBase.Infrastructure.Data.Entities;

public class TagVersion : BaseEntity
{
    public int TagId { get; set; }
    public int BoardId { get; set; }
    public string Name { get; set; }
    public string Color { get; set; }
}
