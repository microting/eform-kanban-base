using System;
using Microting.eFormApi.BasePn.Infrastructure.Database.Base;

namespace Microting.KanbanBase.Infrastructure.Data.Entities;

/// <summary>
/// Audit mirror of <see cref="CardNetworkLog"/>. Property names and types MUST match the entity
/// 1:1 (plus <see cref="CardNetworkLogId"/>): <c>KanbanPnBase.MapVersion</c> copies by reflection
/// on property name and swallows a mismatch into a <c>Console.WriteLine</c>, so a missing or
/// misspelled property here silently drops that column from every audit row.
/// <c>ChromeExtensionCaptureSchemaTests</c> is what actually enforces the parity.
/// </summary>
public class CardNetworkLogVersion : BaseEntity
{
    public int CardNetworkLogId { get; set; }
    public int CardId { get; set; }
    public string? RequestId { get; set; }
    public string? Method { get; set; }
    public string? Url { get; set; }
    public string? DocumentUrl { get; set; }
    public string? ResourceType { get; set; }
    public int? StatusCode { get; set; }
    public string? StatusText { get; set; }
    public string? MimeType { get; set; }
    public string? Protocol { get; set; }
    public string? RemoteIpAddress { get; set; }
    public string? RequestHeadersJson { get; set; }
    public string? ResponseHeadersJson { get; set; }
    public string? RequestBody { get; set; }
    public bool? RequestBodyTruncated { get; set; }
    public string? ResponseBody { get; set; }
    public bool? ResponseBodyBase64 { get; set; }
    public bool? ResponseBodyTruncated { get; set; }
    public long? EncodedDataLength { get; set; }
    public DateTime? StartedAtUtc { get; set; }
    public DateTime? FinishedAtUtc { get; set; }
    public int? DurationMs { get; set; }
    public string? TimingJson { get; set; }
    public string? InitiatorJson { get; set; }
    public bool? FromCache { get; set; }
    public bool? Canceled { get; set; }
    public string? BlockedReason { get; set; }
    public string? FailureText { get; set; }
}
