using System;
using System.Collections.Generic;
using Microting.KanbanBase.Infrastructure.Enums;

namespace Microting.KanbanBase.Infrastructure.Data.Entities;

public class Card : KanbanPnBase
{
    public int BoardId { get; set; }
    public virtual Board Board { get; set; }
    public int ColumnId { get; set; }
    public virtual Column Column { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int Position { get; set; }
    public CardType CardType { get; set; }
    public Priority Priority { get; set; }
    public int? StoryPoints { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? DueDate { get; set; }
    public int? SprintId { get; set; }
    public virtual Sprint? Sprint { get; set; }
    public int? ProjectId { get; set; }
    public virtual Project? Project { get; set; }
    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public virtual ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();
    public virtual ICollection<CardTask> Tasks { get; set; } = new List<CardTask>();
    public virtual ICollection<ActivityLogEntry> ActivityLog { get; set; } = new List<ActivityLogEntry>();
    public virtual ICollection<CardTag> CardTags { get; set; } = new List<CardTag>();
    public virtual ICollection<CardAssignee> Assignees { get; set; } = new List<CardAssignee>();
    public virtual ICollection<CardDependency> PredecessorLinks { get; set; } = new List<CardDependency>();
    public virtual ICollection<CardDependency> SuccessorLinks { get; set; } = new List<CardDependency>();
    public virtual ICollection<CardGitHubLink> GitHubLinks { get; set; } = new List<CardGitHubLink>();

    // Userback feedback metadata
    public FeedbackType FeedbackType { get; set; } = FeedbackType.None;
    public int? Rating { get; set; }
    public int VoteCount { get; set; }
    public string? SubmitterName { get; set; }
    public string? SubmitterEmail { get; set; }
    public CardSource Source { get; set; } = CardSource.Manual;
    public long? UserbackFeedbackId { get; set; }

    public virtual CardCaptureContext? CaptureContext { get; set; }
    public virtual ICollection<CardConsoleLog> ConsoleLogs { get; set; } = new List<CardConsoleLog>();
}
