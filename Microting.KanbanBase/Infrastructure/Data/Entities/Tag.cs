using System.Collections.Generic;

namespace Microting.KanbanBase.Infrastructure.Data.Entities;

public class Tag : KanbanPnBase
{
    public int BoardId { get; set; }
    public virtual Board Board { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Color { get; set; } = "#6B7280";

    public virtual ICollection<CardTag> CardTags { get; set; } = new List<CardTag>();
}
