using Microsoft.EntityFrameworkCore;
using Microting.eFormApi.BasePn.Abstractions;
using Microting.eFormApi.BasePn.Infrastructure.Database.Entities;
using Microting.KanbanBase.Infrastructure.Data.Entities;

namespace Microting.KanbanBase.Infrastructure.Data;

public class KanbanPnDbContext : DbContext, IPluginDbContext
{
    public KanbanPnDbContext() { }
    public KanbanPnDbContext(DbContextOptions<KanbanPnDbContext> options) : base(options) { }

    // Main entity DbSets
    public DbSet<Board> Boards { get; set; }
    public DbSet<Card> Cards { get; set; }
    public DbSet<Column> Columns { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Attachment> Attachments { get; set; }
    public DbSet<CardTask> CardTasks { get; set; }
    public DbSet<Sprint> Sprints { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<CardTag> CardTags { get; set; }
    public DbSet<CardAssignee> CardAssignees { get; set; }
    public DbSet<BoardMember> BoardMembers { get; set; }
    public DbSet<ActivityLogEntry> ActivityLogEntries { get; set; }
    public DbSet<CardDependency> CardDependencies { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<GitHubAppSetting> GitHubAppSettings { get; set; }
    public DbSet<ProjectRepository> ProjectRepositories { get; set; }
    public DbSet<CardGitHubLink> CardGitHubLinks { get; set; }
    public DbSet<CardCaptureContext> CardCaptureContexts { get; set; }
    public DbSet<CardConsoleLog> CardConsoleLogs { get; set; }
    public DbSet<UserbackImportRun> UserbackImportRuns { get; set; }
    public DbSet<UserbackImportLogEntry> UserbackImportLogEntries { get; set; }
    public DbSet<UserbackProjectSyncState> UserbackProjectSyncStates { get; set; }

    // Version entity DbSets
    public DbSet<BoardVersion> BoardVersions { get; set; }
    public DbSet<CardVersion> CardVersions { get; set; }
    public DbSet<ColumnVersion> ColumnVersions { get; set; }
    public DbSet<CommentVersion> CommentVersions { get; set; }
    public DbSet<AttachmentVersion> AttachmentVersions { get; set; }
    public DbSet<CardTaskVersion> CardTaskVersions { get; set; }
    public DbSet<SprintVersion> SprintVersions { get; set; }
    public DbSet<TagVersion> TagVersions { get; set; }
    public DbSet<CardTagVersion> CardTagVersions { get; set; }
    public DbSet<CardAssigneeVersion> CardAssigneeVersions { get; set; }
    public DbSet<BoardMemberVersion> BoardMemberVersions { get; set; }
    public DbSet<ActivityLogEntryVersion> ActivityLogEntryVersions { get; set; }
    public DbSet<CardDependencyVersion> CardDependencyVersions { get; set; }
    public DbSet<ProjectVersion> ProjectVersions { get; set; }
    public DbSet<GitHubAppSettingVersion> GitHubAppSettingVersions { get; set; }
    public DbSet<ProjectRepositoryVersion> ProjectRepositoryVersions { get; set; }
    public DbSet<CardGitHubLinkVersion> CardGitHubLinkVersions { get; set; }
    public DbSet<CardCaptureContextVersion> CardCaptureContextVersions { get; set; }
    public DbSet<CardConsoleLogVersion> CardConsoleLogVersions { get; set; }
    public DbSet<UserbackImportRunVersion> UserbackImportRunVersions { get; set; }
    public DbSet<UserbackImportLogEntryVersion> UserbackImportLogEntryVersions { get; set; }
    public DbSet<UserbackProjectSyncStateVersion> UserbackProjectSyncStateVersions { get; set; }

    // Plugin common tables
    public DbSet<PluginConfigurationValue> PluginConfigurationValues { get; set; }
    public DbSet<PluginConfigurationValueVersion> PluginConfigurationValueVersions { get; set; }
    public DbSet<PluginPermission> PluginPermissions { get; set; }
    public DbSet<PluginGroupPermission> PluginGroupPermissions { get; set; }
    public DbSet<PluginGroupPermissionVersion> PluginGroupPermissionVersions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Board
        modelBuilder.Entity<Board>(entity =>
        {
            entity.Property(e => e.Name).HasMaxLength(255).IsRequired();
            entity.Property(e => e.Description).HasMaxLength(2000);
            entity.HasMany(e => e.Columns).WithOne(e => e.Board).HasForeignKey(e => e.BoardId).OnDelete(DeleteBehavior.Cascade);
            entity.HasMany(e => e.Sprints).WithOne(e => e.Board).HasForeignKey(e => e.BoardId).OnDelete(DeleteBehavior.Cascade);
            entity.HasMany(e => e.Tags).WithOne(e => e.Board).HasForeignKey(e => e.BoardId).OnDelete(DeleteBehavior.Cascade);
            entity.HasMany(e => e.Cards).WithOne(e => e.Board).HasForeignKey(e => e.BoardId).OnDelete(DeleteBehavior.Cascade);
            entity.HasMany(e => e.Members).WithOne(e => e.Board).HasForeignKey(e => e.BoardId).OnDelete(DeleteBehavior.Cascade);
        });

        // Card
        modelBuilder.Entity<Card>(entity =>
        {
            entity.Property(e => e.Title).HasMaxLength(500).IsRequired();
            entity.HasIndex(e => new { e.BoardId, e.ColumnId, e.Position });
            entity.HasIndex(e => new { e.BoardId, e.ColumnId, e.WorkflowState, e.Position });
            entity.HasIndex(e => e.SprintId);
            entity.HasIndex(e => e.ProjectId);
            entity.HasOne(e => e.Column).WithMany(e => e.Cards).HasForeignKey(e => e.ColumnId).OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(e => e.Sprint).WithMany(e => e.Cards).HasForeignKey(e => e.SprintId).OnDelete(DeleteBehavior.SetNull);
            entity.HasOne(e => e.Project).WithMany(e => e.Cards).HasForeignKey(e => e.ProjectId).OnDelete(DeleteBehavior.Restrict);
            entity.HasMany(e => e.Comments).WithOne(e => e.Card).HasForeignKey(e => e.CardId).OnDelete(DeleteBehavior.Cascade);
            entity.HasMany(e => e.Attachments).WithOne(e => e.Card).HasForeignKey(e => e.CardId).OnDelete(DeleteBehavior.Cascade);
            entity.HasMany(e => e.Tasks).WithOne(e => e.Card).HasForeignKey(e => e.CardId).OnDelete(DeleteBehavior.Cascade);
            entity.HasMany(e => e.ActivityLog).WithOne(e => e.Card).HasForeignKey(e => e.CardId).OnDelete(DeleteBehavior.Cascade);
            entity.HasMany(e => e.CardTags).WithOne(e => e.Card).HasForeignKey(e => e.CardId).OnDelete(DeleteBehavior.Cascade);
            entity.HasMany(e => e.Assignees).WithOne(e => e.Card).HasForeignKey(e => e.CardId).OnDelete(DeleteBehavior.Cascade);
            entity.HasMany(e => e.PredecessorLinks).WithOne(e => e.PredecessorCard).HasForeignKey(e => e.PredecessorCardId).OnDelete(DeleteBehavior.Cascade);
            entity.HasMany(e => e.SuccessorLinks).WithOne(e => e.SuccessorCard).HasForeignKey(e => e.SuccessorCardId).OnDelete(DeleteBehavior.Restrict);
            entity.HasIndex(e => e.UserbackFeedbackId);
        });

        // CardCaptureContext (1:1 with Card)
        modelBuilder.Entity<CardCaptureContext>(entity =>
        {
            entity.HasIndex(e => e.CardId).IsUnique();
            entity.HasOne(e => e.Card).WithOne(c => c.CaptureContext).HasForeignKey<CardCaptureContext>(c => c.CardId).OnDelete(DeleteBehavior.Cascade);
            entity.Property(e => e.PageUrl).HasMaxLength(2048);
            entity.Property(e => e.UserAgent).HasMaxLength(1024);
            entity.Property(e => e.BrowserName).HasMaxLength(200);
            entity.Property(e => e.BrowserVersion).HasMaxLength(100);
            entity.Property(e => e.OsName).HasMaxLength(200);
            entity.Property(e => e.OsVersion).HasMaxLength(100);
            entity.Property(e => e.DeviceType).HasMaxLength(100);
            entity.Property(e => e.WindowSize).HasMaxLength(50);
            entity.Property(e => e.ScreenResolution).HasMaxLength(50);
        });

        // CardConsoleLog (1:N with Card)
        modelBuilder.Entity<CardConsoleLog>(entity =>
        {
            entity.HasIndex(e => e.CardId);
            entity.HasOne(e => e.Card).WithMany(c => c.ConsoleLogs).HasForeignKey(e => e.CardId).OnDelete(DeleteBehavior.Cascade);
            entity.Property(e => e.Message).HasMaxLength(4000).IsRequired();
            entity.Property(e => e.Source).HasMaxLength(500);
        });

        // UserbackImportRun
        modelBuilder.Entity<UserbackImportRun>(entity =>
        {
            entity.Property(e => e.ProjectsJson).IsRequired();
            entity.Property(e => e.ErrorMessage).HasMaxLength(4000);
        });

        // UserbackProjectSyncState
        modelBuilder.Entity<UserbackProjectSyncState>(entity =>
        {
            entity.HasIndex(e => e.UserbackProjectId).IsUnique();
        });

        // UserbackImportLogEntry
        modelBuilder.Entity<UserbackImportLogEntry>(entity =>
        {
            // Deliberately NON-unique. A log entry is a per-run audit record, and two rows for
            // one feedback id within one run are reachable (an Imported row is committed, then a
            // downstream fault drops into the per-item catch, which writes a second Failed row).
            entity.HasIndex(e => e.UserbackFeedbackId);
            // (RunId, UserbackFeedbackId) below already carries RunId as its leftmost prefix, so
            // InnoDB would accept it as the index backing the Run FK — this single-column one is
            // NOT required for that. It is kept purely because it is the narrowest index for the
            // "show me one run's log" query, and write cost on an append-only audit table is
            // negligible. Do not justify it as the FK's index.
            entity.HasIndex(e => e.RunId);
            entity.HasIndex(e => new { e.RunId, e.UserbackFeedbackId });
            entity.HasOne(e => e.Run).WithMany(r => r.Entries).HasForeignKey(e => e.RunId).OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(e => e.Card).WithMany().HasForeignKey(e => e.CardId).IsRequired(false).OnDelete(DeleteBehavior.SetNull);
            entity.Property(e => e.ErrorMessage).HasMaxLength(4000);
        });

        // CardDependency
        modelBuilder.Entity<CardDependency>(entity =>
        {
            entity.HasIndex(e => new { e.PredecessorCardId, e.SuccessorCardId }).IsUnique();
        });

        // Project
        modelBuilder.Entity<Project>(entity =>
        {
            entity.Property(e => e.Name).HasMaxLength(500).IsRequired();
            entity.Property(e => e.Description).HasMaxLength(2000);
            entity.HasMany(e => e.Repositories).WithOne(e => e.Project).HasForeignKey(e => e.ProjectId).OnDelete(DeleteBehavior.Cascade);
        });

        // ProjectRepository
        modelBuilder.Entity<ProjectRepository>(entity =>
        {
            entity.Property(e => e.Owner).HasMaxLength(200).IsRequired();
            entity.Property(e => e.Repo).HasMaxLength(200).IsRequired();
            entity.HasIndex(e => new { e.ProjectId, e.Owner, e.Repo }).IsUnique();
        });

        // CardGitHubLink
        modelBuilder.Entity<CardGitHubLink>(entity =>
        {
            entity.HasIndex(e => new { e.CardId, e.ProjectRepositoryId, e.IssueNumber }).IsUnique();
            entity.HasIndex(e => new { e.CardId, e.WorkflowState });
            entity.HasOne(e => e.Card).WithMany(e => e.GitHubLinks).HasForeignKey(e => e.CardId).OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(e => e.ProjectRepository).WithMany().HasForeignKey(e => e.ProjectRepositoryId).OnDelete(DeleteBehavior.Restrict);
        });

        // GitHubAppSetting
        modelBuilder.Entity<GitHubAppSetting>(entity =>
        {
            entity.Property(e => e.AppName).HasMaxLength(255);
            entity.Property(e => e.ClientId).HasMaxLength(255);
        });

        // ActivityLogEntry
        modelBuilder.Entity<ActivityLogEntry>(entity =>
        {
            entity.HasIndex(e => new { e.CardId, e.CreatedAt });
            entity.HasIndex(e => new { e.BoardId, e.CreatedAt });
            entity.Property(e => e.OldValue).HasColumnType("longtext");
            entity.Property(e => e.NewValue).HasColumnType("longtext");
        });

        // CardTag
        modelBuilder.Entity<CardTag>(entity =>
        {
            entity.HasIndex(e => new { e.CardId, e.TagId }).IsUnique();
            entity.HasOne(e => e.Tag).WithMany(e => e.CardTags).HasForeignKey(e => e.TagId).OnDelete(DeleteBehavior.Cascade);
        });

        // CardAssignee
        modelBuilder.Entity<CardAssignee>(entity =>
        {
            entity.HasIndex(e => new { e.CardId, e.UserId }).IsUnique();
        });

        // Comment
        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasIndex(e => e.CardId);
            entity.HasIndex(e => new { e.CardId, e.WorkflowState });
            // Deliberately NON-unique (corrections C5/C6). KanbanPnBase.Delete soft-deletes, so a
            // removed comment keeps its UserbackCommentId and its index slot, and MySQL has no
            // filtered indexes — a unique index would permanently block re-importing a comment
            // that had once been deleted. Re-import uniqueness is enforced in code via a
            // FirstOrDefaultAsync lookup plus a guarded insert. Card-scoped, because board
            // resolution is by name and the same upstream comment can legitimately land on more
            // than one board.
            entity.HasIndex(e => new { e.CardId, e.UserbackCommentId });
        });

        // Attachment
        modelBuilder.Entity<Attachment>(entity =>
        {
            entity.HasIndex(e => e.CardId);
            entity.HasIndex(e => new { e.CardId, e.WorkflowState });
            // SourceUrl is intentionally unbounded (longtext) — signed Userback CDN URLs overflow
            // any workable varchar, and STRICT_TRANS_TABLES turns an over-length value into a
            // throw the importer's per-media catch would swallow as a warning. Dedupe on the
            // fixed-width SHA-256 digest instead; utf8mb4 varchar(2048) in an index would exceed
            // InnoDB's 3072-byte limit.
            entity.Property(e => e.SourceUrlHash).HasMaxLength(64).IsFixedLength();
            entity.HasIndex(e => new { e.CardId, e.SourceUrlHash });
        });
    }
}
