namespace Microting.KanbanBase.Infrastructure.Enums;

/// <summary>
/// Severity of a captured console entry.
/// <para>
/// These are PERSISTED INTEGERS — <c>CardConsoleLogs.Level</c> is an <c>int</c> column. Append new
/// members only; renumbering an existing one silently reinterprets every stored row.
/// </para>
/// <para>
/// Mapping from CDP, which has a wider vocabulary than this enum:
/// <list type="bullet">
/// <item><c>Runtime.consoleAPICalled.type</c> — <c>log</c>/<c>dir</c>/<c>dirxml</c>/<c>table</c>/
/// <c>group</c>* /<c>count</c>/<c>timeEnd</c>/<c>clear</c> collapse onto <see cref="Log"/>,
/// <c>info</c> onto <see cref="Info"/>, <c>warning</c> onto <see cref="Warn"/>, <c>error</c>/
/// <c>assert</c> onto <see cref="Error"/>, <c>debug</c> onto <see cref="Debug"/>, <c>trace</c>
/// onto <see cref="Debug"/>. The original CDP type is not preserved as a column — the collapsed
/// members are a deliberate simplification, and the raw arguments survive in
/// <c>CardConsoleLog.ArgsJson</c>.</item>
/// <item><c>Log.entryAdded.level</c> — <c>verbose</c> maps onto <see cref="Verbose"/>, the other
/// three onto <see cref="Info"/>/<see cref="Warn"/>/<see cref="Error"/>.</item>
/// <item><c>Runtime.exceptionThrown</c> carries no level at all; it maps onto
/// <see cref="Exception"/> so an uncaught throw is distinguishable from a hand-written
/// <c>console.error</c>.</item>
/// </list>
/// </para>
/// </summary>
public enum ConsoleLogLevel
{
    Log = 0,
    Info = 1,
    Warn = 2,
    Error = 3,
    Debug = 4,

    /// <summary>CDP <c>Log.entryAdded</c> level <c>verbose</c>. Appended 2026-10 — see #26.</summary>
    Verbose = 5,

    /// <summary>
    /// CDP <c>Runtime.exceptionThrown</c> — an uncaught exception or unhandled rejection, not a
    /// console API call. Appended 2026-10 — see #26.
    /// </summary>
    Exception = 6
}
