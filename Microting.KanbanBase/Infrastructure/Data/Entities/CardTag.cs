using Microting.KanbanBase.Infrastructure.Enums;

namespace Microting.KanbanBase.Infrastructure.Data.Entities;

public class CardTag : KanbanPnBase
{
    public int CardId { get; set; }
    public virtual Card Card { get; set; }
    public int TagId { get; set; }
    public virtual Tag Tag { get; set; }

    /// <summary>
    /// Who owns this card/tag link. A sync may only add or remove rows it owns
    /// (<see cref="CardSource.Userback"/>); rows a human added stay untouched.
    /// </summary>
    public CardSource Source { get; set; } = CardSource.Manual;
}
