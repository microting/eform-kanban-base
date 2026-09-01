using Microting.eFormApi.BasePn.Infrastructure.Database.Base;
using Microting.KanbanBase.Infrastructure.Enums;

namespace Microting.KanbanBase.Infrastructure.Data.Entities;

public class CardTagVersion : BaseEntity
{
    public int CardTagId { get; set; }
    public int CardId { get; set; }
    public int TagId { get; set; }
    public CardSource Source { get; set; } = CardSource.Manual;
}
