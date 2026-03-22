using Microting.eFormApi.BasePn.Infrastructure.Database.Base;
using Microting.KanbanBase.Infrastructure.Enums;

namespace Microting.KanbanBase.Infrastructure.Data.Entities;

public class ColumnVersion : BaseEntity
{
    public int ColumnId { get; set; }
    public int BoardId { get; set; }
    public string Name { get; set; }
    public int Position { get; set; }
    public ColumnType ColumnType { get; set; }
    public int? WipLimit { get; set; }
    public string Color { get; set; }
}
