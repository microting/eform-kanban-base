using Microting.KanbanBase.Infrastructure.Enums;

namespace Microting.KanbanBase.Infrastructure.Data.Entities;

public class CardDependency : KanbanPnBase
{
    public int PredecessorCardId { get; set; }
    public virtual Card PredecessorCard { get; set; }
    public int SuccessorCardId { get; set; }
    public virtual Card SuccessorCard { get; set; }
    public DependencyType DependencyType { get; set; }
}
