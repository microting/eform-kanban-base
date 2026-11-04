namespace Microting.KanbanBase.Infrastructure.Data.Entities;

/// <summary>A workspace's optional answers webhook (spec 2026-10-30 §7): signed HMAC-SHA256 POSTs. The secret is stored
/// AES-encrypted (Kanban.Pn's AesEncryptionHelper); one row per workspace, disabled rather than deleted.</summary>
public class WorkspaceWebhook : KanbanPnBase
{
    public int WorkspaceId { get; set; }
    public virtual Workspace Workspace { get; set; } = null!;
    public string Url { get; set; } = string.Empty;
    public string SecretCipher { get; set; } = string.Empty;
    public string SecretIv { get; set; } = string.Empty;
    public bool Enabled { get; set; }
}
