using System;
using Microting.KanbanBase.Infrastructure.Enums;

namespace Microting.KanbanBase.Infrastructure.Data.Entities;

public class CardConsoleLog : KanbanPnBase
{
    public int CardId { get; set; }
    public virtual Card Card { get; set; }
    public ConsoleLogLevel Level { get; set; }
    public string Message { get; set; } = string.Empty;
    public string? Source { get; set; }
    public DateTime? Timestamp { get; set; }
}
