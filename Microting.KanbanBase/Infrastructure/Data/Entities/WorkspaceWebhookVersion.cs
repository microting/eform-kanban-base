using Microting.eFormApi.BasePn.Infrastructure.Database.Base;

namespace Microting.KanbanBase.Infrastructure.Data.Entities;

public class WorkspaceWebhookVersion : BaseEntity
{
    public int WorkspaceWebhookId { get; set; }
    public int WorkspaceId { get; set; }
    public string Url { get; set; } = string.Empty;
    public string SecretCipher { get; set; } = string.Empty;
    public string SecretIv { get; set; } = string.Empty;
    public bool Enabled { get; set; }
}
