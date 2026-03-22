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
            entity.Property(e => e.GitHubOwner).HasMaxLength(200);
            entity.Property(e => e.GitHubRepo).HasMaxLength(200);
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
    }
}
