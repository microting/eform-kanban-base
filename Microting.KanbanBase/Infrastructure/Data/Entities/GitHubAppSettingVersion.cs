using Microting.eFormApi.BasePn.Infrastructure.Database.Base;

namespace Microting.KanbanBase.Infrastructure.Data.Entities;

public class GitHubAppSettingVersion : BaseEntity
{
    public int GitHubAppSettingId { get; set; }
    public long AppId { get; set; }
    public string AppName { get; set; } = string.Empty;
    public string PrivateKeyEncrypted { get; set; } = string.Empty;
    public string PrivateKeyIv { get; set; } = string.Empty;
    public string ClientId { get; set; } = string.Empty;
    public string? ClientSecretEncrypted { get; set; }
    public string? ClientSecretIv { get; set; }
    public string? WebhookSecret { get; set; }
}
