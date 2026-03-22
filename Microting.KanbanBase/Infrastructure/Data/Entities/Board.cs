using System.Collections.Generic;

namespace Microting.KanbanBase.Infrastructure.Data.Entities;

public class Board : KanbanPnBase
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    public virtual ICollection<Column> Columns { get; set; } = new List<Column>();
    public virtual ICollection<Sprint> Sprints { get; set; } = new List<Sprint>();
    public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();
    public virtual ICollection<Card> Cards { get; set; } = new List<Card>();
    public virtual ICollection<BoardMember> Members { get; set; } = new List<BoardMember>();
}
