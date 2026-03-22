using System;
using System.Collections.Generic;
using Microting.KanbanBase.Infrastructure.Enums;

namespace Microting.KanbanBase.Infrastructure.Data.Entities;

public class Sprint : KanbanPnBase
{
    public int BoardId { get; set; }
    public virtual Board Board { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Goal { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public SprintStatus Status { get; set; }

    public virtual ICollection<Card> Cards { get; set; } = new List<Card>();
}
