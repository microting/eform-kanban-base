#nullable enable
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microting.KanbanBase.Infrastructure.Data;
using NUnit.Framework;
using Testcontainers.MariaDb;

namespace Microting.KanbanBase.Tests;

/// <summary>The Workspaces migration on a database that already holds data: everything lands in workspace 1 and
/// tenant links become customers.</summary>
[TestFixture]
public class WorkspacesMigrationDataTests
{
    private readonly MariaDbContainer _db = new MariaDbBuilder("mariadb:10.8")
        .WithUsername("root").WithPassword("secretpassword").Build();

    [OneTimeSetUp] public Task Start() => _db.StartAsync();
    [OneTimeTearDown] public async Task Stop() => await _db.DisposeAsync();

    private KanbanPnDbContext Context()
    {
        var cs = _db.GetConnectionString();
        return new KanbanPnDbContext(new DbContextOptionsBuilder<KanbanPnDbContext>()
            .UseMySql(cs, ServerVersion.AutoDetect(cs)).Options);
    }

    private static string Before(KanbanPnDbContext ctx)
    {
        var all = ctx.Database.GetMigrations().ToList();
        return all[all.FindIndex(m => m.EndsWith("_Workspaces")) - 1];
    }

    [Test]
    public async Task ExistingRows_MoveToWorkspaceOne_AndTenantsBecomeCustomers()
    {
        await using var ctx = Context();
        var migrator = ctx.GetService<IMigrator>();
        await migrator.MigrateAsync(Before(ctx));

        const string now = "UTC_TIMESTAMP(6)";
        await ctx.Database.ExecuteSqlRawAsync($@"
INSERT INTO Boards (Id, Name, CreatedAt, UpdatedAt, WorkflowState, CreatedByUserId, UpdatedByUserId, Version) VALUES (10, 'B', {now}, {now}, 'created', 0, 0, 1);
INSERT INTO Columns (Id, BoardId, Name, Color, Position, ColumnType, CreatedAt, UpdatedAt, WorkflowState, CreatedByUserId, UpdatedByUserId, Version) VALUES (20, 10, 'Todo', '#000', 0, 0, {now}, {now}, 'created', 0, 0, 1);
INSERT INTO Cards (Id, BoardId, ColumnId, Title, Position, CardType, Priority, FeedbackType, VoteCount, Source, Attention, CreatedAt, UpdatedAt, WorkflowState, CreatedByUserId, UpdatedByUserId, Version)
  VALUES (30, 10, 20, 'C1', 0, 0, 0, 0, 0, 0, 0, {now}, {now}, 'created', 0, 0, 1),
         (31, 10, 20, 'C2', 1, 0, 0, 0, 0, 0, 0, {now}, {now}, 'created', 0, 0, 1);
INSERT INTO CardTenants (CardId, CustomerNo, OrganizationName, FetchedAt, CreatedAt, UpdatedAt, WorkflowState, CreatedByUserId, UpdatedByUserId, Version)
  VALUES (30, '922', NULL, {now}, {now}, {now}, 'created', 0, 0, 1),
         (31, '922', 'Vestjysk Kontrol A/S', {now}, {now}, {now}, 'created', 0, 0, 1),
         (30, '1001', 'Fresh Name', {now}, {now}, {now}, 'created', 0, 0, 1),
         (31, '1001', 'Stale Name', {now} - INTERVAL 1 DAY, {now}, {now}, 'created', 0, 0, 1),
         (30, '555', 'Gone', {now}, {now}, {now}, 'removed', 0, 0, 1),
         (30, '777', 'No State', {now}, {now}, {now}, NULL, 0, 0, 1);
INSERT INTO Projects (Id, Name, CreatedAt, UpdatedAt, WorkflowState, CreatedByUserId, UpdatedByUserId, Version) VALUES (40, 'P', {now}, {now}, 'created', 0, 0, 1);
INSERT INTO IntegrationApiKeys (Id, Name, KeyPrefix, KeyHash, Scope, ServiceUserId, CreatedAt, UpdatedAt, WorkflowState, CreatedByUserId, UpdatedByUserId, Version)
  VALUES (50, 'triage', 'kbn_abcd', REPEAT('a', 64), 'sentry-triage', 7, {now}, {now}, 'created', 0, 0, 1);");

        // To the latest migration, not exactly to _Workspaces: the rows above were inserted before it, so its
        // backfill is still what is exercised, and later migrations adding columns do not break the queries below.
        await ctx.Database.MigrateAsync();

        Assert.That(await ctx.Workspaces.Where(w => w.Id == 1).Select(w => new { w.Id, w.Name, w.Slug }).SingleAsync(),
            Is.EqualTo(new { Id = 1, Name = "Microting", Slug = "microting" }));
        Assert.That(await ctx.Boards.Where(b => b.Id == 10).Select(b => b.WorkspaceId).SingleAsync(), Is.EqualTo(1));
        Assert.That(await ctx.Projects.Where(p => p.Id == 40).Select(p => p.WorkspaceId).SingleAsync(), Is.EqualTo(1));
        Assert.That(await ctx.IntegrationApiKeys.Where(k => k.Id == 50).Select(k => k.WorkspaceId).SingleAsync(), Is.EqualTo(1));

        var customers = await ctx.Customers.OrderBy(c => c.CustomerNo).ToListAsync();
        // '555' only has a removed link: no customer is made for it. '777' has a NULL state, which is not removed.
        // '1001' takes the most recently fetched name, not the one on the newest row.
        Assert.That(customers.Select(c => (c.WorkspaceId, c.CustomerNo, c.Name)), Is.EqualTo(new[]
        {
            (1, (string?)"1001", "Fresh Name"),
            (1, (string?)"777", "No State"),
            (1, (string?)"922", "Vestjysk Kontrol A/S"),
        }));
        var c922 = customers.Single(c => c.CustomerNo == "922").Id;
        Assert.That(await ctx.CardTenants.Where(t => t.CustomerNo == "922").Select(t => t.CustomerId).ToListAsync(),
            Is.EqualTo(new int?[] { c922, c922 }));
        Assert.That(await ctx.CardTenants.Where(t => t.CustomerNo == "777").Select(t => t.CustomerId).SingleAsync(),
            Is.EqualTo(customers.Single(c => c.CustomerNo == "777").Id));
        Assert.That(await ctx.CardTenants.Where(t => t.CustomerNo == "555").Select(t => t.CustomerId).SingleAsync(), Is.Null);

        // Version 1 snapshots exist for the seeded workspace and each backfilled customer.
        Assert.That(await ctx.WorkspaceVersions.Where(v => v.WorkspaceId == 1).Select(v => new { v.Name, v.Slug, v.Version }).SingleAsync(),
            Is.EqualTo(new { Name = "Microting", Slug = "microting", Version = 1 }));
        Assert.That(await ctx.CustomerVersions.OrderBy(v => v.CustomerNo).Select(v => new { v.CustomerId, v.Name, v.Version }).ToListAsync(),
            Is.EqualTo(customers.Select(c => new { CustomerId = c.Id, c.Name, Version = 1 })));

        // The column default (1) only served the backfill: new boards, projects and keys must name their workspace.
        await ctx.Database.OpenConnectionAsync();
        await using var cmd = ctx.Database.GetDbConnection().CreateCommand();
        cmd.CommandText = @"SELECT COUNT(*) FROM information_schema.COLUMNS
WHERE TABLE_SCHEMA = DATABASE() AND COLUMN_NAME = 'WorkspaceId'
  AND TABLE_NAME IN ('Boards', 'Projects', 'IntegrationApiKeys') AND COLUMN_DEFAULT IS NULL";
        Assert.That(Convert.ToInt32(await cmd.ExecuteScalarAsync()), Is.EqualTo(3));

        // The backfill's temporary index is gone again.
        cmd.CommandText = @"SELECT COUNT(*) FROM information_schema.STATISTICS
WHERE TABLE_SCHEMA = DATABASE() AND TABLE_NAME = 'CardTenants' AND INDEX_NAME = 'IX_CardTenants_CustomerNo_Backfill'";
        Assert.That(Convert.ToInt32(await cmd.ExecuteScalarAsync()), Is.EqualTo(0));
    }
}
