using System;

namespace Microting.KanbanBase.Infrastructure.Data.Entities;

/// <summary>A tenant (eForm customer) affected by the report on this card.</summary>
public class CardTenant : KanbanPnBase
{
    public int CardId { get; set; }
    public virtual Card Card { get; set; }
    public string CustomerNo { get; set; } = string.Empty;
    public string? OrganizationName { get; set; }
    public string? CvrNo { get; set; }
    public string? PaymentStatus { get; set; }
    public DateTime FetchedAt { get; set; }
}
