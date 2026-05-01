using System;
using Microting.eFormApi.BasePn.Infrastructure.Database.Base;
using Microting.KanbanBase.Infrastructure.Enums;

namespace Microting.KanbanBase.Infrastructure.Data.Entities;

public class CardConsoleLogVersion : BaseEntity
{
    public int CardConsoleLogId { get; set; }
    public int CardId { get; set; }
    public ConsoleLogLevel Level { get; set; }
    public string Message { get; set; } = string.Empty;
    public string? Source { get; set; }
    public DateTime? Timestamp { get; set; }
}
