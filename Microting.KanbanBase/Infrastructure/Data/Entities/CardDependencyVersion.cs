using Microting.eFormApi.BasePn.Infrastructure.Database.Base;
using Microting.KanbanBase.Infrastructure.Enums;

namespace Microting.KanbanBase.Infrastructure.Data.Entities;

public class CardDependencyVersion : BaseEntity
{
    public int CardDependencyId { get; set; }
    public int PredecessorCardId { get; set; }
    public int SuccessorCardId { get; set; }
    public DependencyType DependencyType { get; set; }
}
