using System;

namespace Microting.KanbanBase.Infrastructure.Data.Entities;

/// <summary>
/// A machine API key for the kanban integration endpoints. Only the SHA-256 hash is stored;
/// the key acts as its own service user (<see cref="ServiceUserId"/>, an eForm host user id).
/// </summary>
public class IntegrationApiKey : KanbanPnBase
{
    public string Name { get; set; } = string.Empty;
    public string KeyPrefix { get; set; } = string.Empty;
    public string KeyHash { get; set; } = string.Empty;
    public string Scope { get; set; } = "sentry-triage";
    public int ServiceUserId { get; set; }
    public DateTime? LastUsedAt { get; set; }
    public DateTime? RevokedAt { get; set; }
}
