using System;
using Microting.eFormApi.BasePn.Infrastructure.Database.Base;
using Microting.KanbanBase.Infrastructure.Enums;

namespace Microting.KanbanBase.Infrastructure.Data.Entities;

public class UserbackImportRunVersion : BaseEntity
{
    public int UserbackImportRunId { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime? FinishedAt { get; set; }
    public int RunByUserId { get; set; }
    public string ProjectsJson { get; set; } = string.Empty;
    public UserbackImportRunStatus Status { get; set; } = UserbackImportRunStatus.Pending;
    public string? ErrorMessage { get; set; }
    public int CardsImported { get; set; }
    public int AttachmentsImported { get; set; }
    public int CommentsImported { get; set; }
}
