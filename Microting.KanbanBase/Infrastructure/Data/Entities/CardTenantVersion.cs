using System;
using Microting.eFormApi.BasePn.Infrastructure.Database.Base;

namespace Microting.KanbanBase.Infrastructure.Data.Entities;

public class CardTenantVersion : BaseEntity
{
    public int CardTenantId { get; set; }
    public int CardId { get; set; }
    public string CustomerNo { get; set; } = string.Empty;
    public string? OrganizationName { get; set; }
    public string? CvrNo { get; set; }
    public string? PaymentStatus { get; set; }
    public DateTime FetchedAt { get; set; }
    public int? CustomerId { get; set; }
}
