using System;

namespace Microting.KanbanBase.Infrastructure.Data.Entities;

/// <summary>
/// One captured HTTP request/response pair on a card, collected by the Chrome extension via
/// <c>chrome.debugger</c> and the CDP <c>Network</c> domain.
/// <para>
/// A row is the flattened join of the whole CDP event sequence for one <c>requestId</c>:
/// <c>Network.requestWillBeSent</c> → <c>Network.responseReceived</c> →
/// <c>Network.loadingFinished</c>, or → <c>Network.loadingFailed</c>. The extension buffers the
/// events in memory for the duration of the capture and POSTs one row per request, so nothing
/// here is written incrementally.
/// </para>
/// <para>
/// EVERYTHING except <see cref="CardId"/> is nullable, deliberately: which CDP events a request
/// produced depends on how it ended. A request that failed has no <see cref="StatusCode"/>; one
/// served from cache has no wire timings; one still in flight when the reporter hit Stop has only
/// the <c>requestWillBeSent</c> half.
/// </para>
/// <para>
/// Headers and bodies live INLINE on this row rather than in a separate body table — see the PR
/// for #26 for the reasoning. The short version: InnoDB DYNAMIC row format stores <c>longtext</c>
/// off-page with a 20-byte pointer in the clustered index, so a query that does not select the
/// body columns never faults the overflow pages in, which is the only thing a split table would
/// have bought.
/// </para>
/// </summary>
public class CardNetworkLog : KanbanPnBase
{
    public int CardId { get; set; }
    public virtual Card Card { get; set; }

    /// <summary>
    /// The CDP <c>requestId</c>. Unique only within one debugger session, so it is NOT a key —
    /// it exists to correlate this row with <see cref="CardConsoleLog.RequestId"/> and with the
    /// redirect chain of the same capture.
    /// </summary>
    public string? RequestId { get; set; }

    public string? Method { get; set; }

    /// <summary>
    /// Full request URL including the query string. Intentionally unbounded (longtext), same
    /// reasoning as <see cref="Attachment.SourceUrl"/>: signed URLs and long query strings exceed
    /// any workable varchar, and utf8mb4 varchar(2048) could not be indexed anyway (InnoDB's
    /// 3072-byte index limit). NEVER put an index on this column.
    /// </summary>
    public string? Url { get; set; }

    /// <summary>
    /// CDP <c>requestWillBeSent.documentURL</c> — the page the request was made from, which is
    /// what tells a reader *where in the app* the failure happened. Unbounded (longtext), same
    /// reasoning as <see cref="Url"/>.
    /// </summary>
    public string? DocumentUrl { get; set; }

    /// <summary>CDP <c>Network.ResourceType</c> — <c>XHR</c>, <c>Fetch</c>, <c>Document</c>, …</summary>
    public string? ResourceType { get; set; }

    /// <summary>NULL when the request never got a response (see <see cref="FailureText"/>).</summary>
    public int? StatusCode { get; set; }

    public string? StatusText { get; set; }
    public string? MimeType { get; set; }

    /// <summary>CDP <c>response.protocol</c> — <c>h2</c>, <c>http/1.1</c>, …</summary>
    public string? Protocol { get; set; }

    /// <summary>
    /// CDP <c>response.remoteIPAddress</c>. Sized for a bracketed IPv6 literal. Worth keeping for
    /// a clustered deployment: it is the cheapest answer to "which pod served the one that 500'd".
    /// </summary>
    public string? RemoteIpAddress { get; set; }

    /// <summary>Request headers as a JSON object. Unbounded (longtext).</summary>
    public string? RequestHeadersJson { get; set; }

    /// <summary>Response headers as a JSON object. Unbounded (longtext).</summary>
    public string? ResponseHeadersJson { get; set; }

    /// <summary>
    /// Request body (CDP <c>request.postData</c> / <c>postDataEntries</c>). Unbounded (longtext),
    /// but the CLIENT is expected to cap it — see <see cref="RequestBodyTruncated"/>.
    /// </summary>
    public string? RequestBody { get; set; }

    /// <summary>
    /// True when the capture client cut <see cref="RequestBody"/> short at its own size cap, so a
    /// reader can tell a deliberately dropped tail from a genuinely short body. NULL means the
    /// client did not say.
    /// </summary>
    public bool? RequestBodyTruncated { get; set; }

    /// <summary>
    /// Response body from CDP <c>Network.getResponseBody</c>. Unbounded (longtext). May be absent
    /// for three different reasons — not captured, over the client's cap, or binary — which is
    /// why <see cref="ResponseBodyTruncated"/> and <see cref="ResponseBodyBase64"/> exist rather
    /// than leaving a reader to guess from an empty string.
    /// </summary>
    public string? ResponseBody { get; set; }

    /// <summary>
    /// CDP <c>getResponseBody</c> returns <c>base64Encoded</c> alongside the body; true means
    /// <see cref="ResponseBody"/> is base64 of binary bytes, not text.
    /// </summary>
    public bool? ResponseBodyBase64 { get; set; }

    /// <summary>
    /// True when the capture client cut <see cref="ResponseBody"/> short at its own size cap.
    /// Distinguishes "empty response" from "we dropped it".
    /// </summary>
    public bool? ResponseBodyTruncated { get; set; }

    /// <summary>
    /// CDP <c>loadingFinished.encodedDataLength</c> — bytes on the wire. This is the true response
    /// size even when <see cref="ResponseBody"/> was truncated or never captured.
    /// </summary>
    public long? EncodedDataLength { get; set; }

    public DateTime? StartedAtUtc { get; set; }
    public DateTime? FinishedAtUtc { get; set; }

    /// <summary>
    /// Wall-clock duration. Derivable from <see cref="StartedAtUtc"/>/<see cref="FinishedAtUtc"/>,
    /// but stored because CDP's monotonic timestamps are sub-millisecond and survive the clock
    /// skew that converting both ends to UTC wall time introduces.
    /// </summary>
    public int? DurationMs { get; set; }

    /// <summary>
    /// The full CDP <c>Network.ResourceTiming</c> object as JSON (dns/connect/ssl/send/receive
    /// phase offsets). Kept as JSON rather than ~15 columns: nothing queries an individual phase
    /// in SQL, a reader wants the whole waterfall or none of it. Unbounded (longtext).
    /// </summary>
    public string? TimingJson { get; set; }

    /// <summary>
    /// CDP <c>requestWillBeSent.initiator</c> as JSON — <c>type</c> (parser/script/preload/other)
    /// plus the originating stack. This is what answers "what in our code fired this request".
    /// Unbounded (longtext).
    /// </summary>
    public string? InitiatorJson { get; set; }

    /// <summary>CDP <c>response.fromDiskCache</c> / <c>fromPrefetchCache</c>.</summary>
    public bool? FromCache { get; set; }

    /// <summary>CDP <c>loadingFailed.canceled</c> — a cancelled request is not a defect.</summary>
    public bool? Canceled { get; set; }

    /// <summary>
    /// CDP <c>loadingFailed.blockedReason</c> (<c>mixed-content</c>, <c>csp</c>,
    /// <c>corp-not-same-origin</c>, …). Separate from <see cref="FailureText"/> because a browser
    /// *block* and a network *error* are different bugs with the same empty status code.
    /// </summary>
    public string? BlockedReason { get; set; }

    /// <summary>
    /// CDP <c>loadingFailed.errorText</c> — a Chrome net error string such as
    /// <c>net::ERR_CONNECTION_REFUSED</c>. Non-NULL implies <see cref="StatusCode"/> is NULL.
    /// </summary>
    public string? FailureText { get; set; }
}
