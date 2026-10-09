using System;
using Microting.KanbanBase.Infrastructure.Enums;

namespace Microting.KanbanBase.Infrastructure.Data.Entities;

/// <summary>
/// One captured devtools console entry on a card. Sourced from CDP
/// <c>Runtime.consoleAPICalled</c>, <c>Runtime.exceptionThrown</c> and <c>Log.entryAdded</c>,
/// collected by the Chrome extension via <c>chrome.debugger</c>.
/// <para>
/// Every field added for the extension is nullable, so the pre-existing writer
/// (<c>UserbackImportService</c>, which sets only <see cref="CardId"/>, <see cref="Level"/>,
/// <see cref="Message"/>, <see cref="Source"/> and <see cref="Timestamp"/>) keeps compiling and
/// keeps producing valid rows.
/// </para>
/// </summary>
public class CardConsoleLog : KanbanPnBase
{
    public int CardId { get; set; }
    public virtual Card Card { get; set; }
    public ConsoleLogLevel Level { get; set; }

    /// <summary>
    /// The rendered entry text. Intentionally unbounded (longtext): a single
    /// <c>console.log(someBigObject)</c> renders well past the varchar(4000) this used to be, and
    /// under STRICT_TRANS_TABLES an over-length value throws instead of truncating — which, in
    /// the extension's bulk POST, would reject the whole batch.
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Origin of the entry: the script URL for <c>consoleAPICalled</c>/<c>exceptionThrown</c>, or
    /// <c>Log.entryAdded.url</c>. Intentionally unbounded (longtext) for the same reason as
    /// <see cref="Attachment.SourceUrl"/> — URLs overflow any workable varchar, and this used to
    /// be varchar(500). Never index this column.
    /// </summary>
    public string? Source { get; set; }

    public DateTime? Timestamp { get; set; }

    /// <summary>
    /// Full CDP <c>StackTrace</c>, rendered. Unbounded (longtext) — a framework stack trace blows
    /// past any workable varchar.
    /// </summary>
    public string? StackTrace { get; set; }

    /// <summary>1-based line number of the top call frame, where CDP supplies one.</summary>
    public int? LineNumber { get; set; }

    /// <summary>1-based column number of the top call frame, where CDP supplies one.</summary>
    public int? ColumnNumber { get; set; }

    /// <summary>
    /// The serialised <c>RemoteObject[]</c> from <c>Runtime.consoleAPICalled.args</c>, so a
    /// multi-argument <c>console.log(a, b, c)</c> is not flattened into one lossy string the way
    /// <see cref="Message"/> necessarily is. Unbounded (longtext).
    /// </summary>
    public string? ArgsJson { get; set; }

    /// <summary>
    /// CDP <c>Log.entryAdded.networkRequestId</c> — correlates a console entry that Chrome itself
    /// emitted about a request (a CORS rejection, a mixed-content block, a 4xx/5xx console
    /// warning) with the matching <see cref="CardNetworkLog.RequestId"/> on the same card. NULL
    /// for entries with no associated request, which is most of them.
    /// </summary>
    public string? RequestId { get; set; }
}
