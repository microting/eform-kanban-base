using System;
using Microting.eFormApi.BasePn.Infrastructure.Database.Base;
using Microting.KanbanBase.Infrastructure.Enums;

namespace Microting.KanbanBase.Infrastructure.Data.Entities;

/// <summary>
/// Audit mirror of <see cref="CardConsoleLog"/>. Property names and types MUST match the entity
/// 1:1 (plus <see cref="CardConsoleLogId"/>): <c>KanbanPnBase.MapVersion</c> copies by reflection
/// on property name and swallows a mismatch into a <c>Console.WriteLine</c>, so a missing or
/// misspelled property here silently drops that column from every audit row.
/// </summary>
public class CardConsoleLogVersion : BaseEntity
{
    public int CardConsoleLogId { get; set; }
    public int CardId { get; set; }
    public ConsoleLogLevel Level { get; set; }
    public string Message { get; set; } = string.Empty;
    public string? Source { get; set; }
    public DateTime? Timestamp { get; set; }
    public string? StackTrace { get; set; }
    public int? LineNumber { get; set; }
    public int? ColumnNumber { get; set; }
    public string? ArgsJson { get; set; }
    public string? RequestId { get; set; }
}
