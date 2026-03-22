using Microting.eFormApi.BasePn.Infrastructure.Database.Base;

namespace Microting.KanbanBase.Infrastructure.Data.Entities;

public class BoardVersion : BaseEntity
{
    public int BoardId { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
}
