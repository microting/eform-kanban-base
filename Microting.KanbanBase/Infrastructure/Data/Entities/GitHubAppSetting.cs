namespace Microting.KanbanBase.Infrastructure.Data.Entities;

public class GitHubAppSetting : KanbanPnBase
{
    public long AppId { get; set; }
    public string AppName { get; set; } = string.Empty;
    public string PrivateKeyEncrypted { get; set; } = string.Empty;
    public string PrivateKeyIv { get; set; } = string.Empty;
    public string ClientId { get; set; } = string.Empty;
    public string? ClientSecretEncrypted { get; set; }
    public string? ClientSecretIv { get; set; }
    public string? WebhookSecret { get; set; }
}
