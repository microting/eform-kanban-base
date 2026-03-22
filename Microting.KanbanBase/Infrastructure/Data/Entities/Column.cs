using System.Collections.Generic;
using Microting.KanbanBase.Infrastructure.Enums;

namespace Microting.KanbanBase.Infrastructure.Data.Entities;

public class Column : KanbanPnBase
{
    public int BoardId { get; set; }
    public virtual Board Board { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Position { get; set; }
    public ColumnType ColumnType { get; set; }
    public int? WipLimit { get; set; }
    public string Color { get; set; } = "#6B7280";

    public virtual ICollection<Card> Cards { get; set; } = new List<Card>();
}
