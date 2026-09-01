using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Microting.KanbanBase.Migrations
{
    /// <inheritdoc />
    public partial class UserbackSyncSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserbackImportLogEntries_UserbackFeedbackId_Status",
                table: "UserbackImportLogEntries");

            migrationBuilder.AddColumn<int>(
                name: "CardsCreated",
                table: "UserbackImportRunVersions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CardsSkipped",
                table: "UserbackImportRunVersions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CardsSoftDeleted",
                table: "UserbackImportRunVersions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CardsUpdated",
                table: "UserbackImportRunVersions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastHeartbeatAt",
                table: "UserbackImportRunVersions",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MediaFailed",
                table: "UserbackImportRunVersions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Mode",
                table: "UserbackImportRunVersions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalToProcess",
                table: "UserbackImportRunVersions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CardsCreated",
                table: "UserbackImportRuns",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CardsSkipped",
                table: "UserbackImportRuns",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CardsSoftDeleted",
                table: "UserbackImportRuns",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CardsUpdated",
                table: "UserbackImportRuns",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastHeartbeatAt",
                table: "UserbackImportRuns",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MediaFailed",
                table: "UserbackImportRuns",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Mode",
                table: "UserbackImportRuns",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalToProcess",
                table: "UserbackImportRuns",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<long>(
                name: "UserbackCommentId",
                table: "CommentVersions",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UserbackCommentId",
                table: "Comments",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Source",
                table: "CardTagVersions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Source",
                table: "CardTags",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "SourceUrl",
                table: "AttachmentVersions",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "SourceUrlHash",
                table: "AttachmentVersions",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "SourceUrl",
                table: "Attachments",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "SourceUrlHash",
                table: "Attachments",
                type: "char(64)",
                fixedLength: true,
                maxLength: 64,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UserbackProjectSyncStates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserbackProjectId = table.Column<int>(type: "int", nullable: false),
                    LastSyncedModifiedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    WorkflowState = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: false),
                    UpdatedByUserId = table.Column<int>(type: "int", nullable: false),
                    Version = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserbackProjectSyncStates", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UserbackProjectSyncStateVersions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserbackProjectSyncStateId = table.Column<int>(type: "int", nullable: false),
                    UserbackProjectId = table.Column<int>(type: "int", nullable: false),
                    LastSyncedModifiedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    WorkflowState = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: false),
                    UpdatedByUserId = table.Column<int>(type: "int", nullable: false),
                    Version = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserbackProjectSyncStateVersions", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_UserbackImportLogEntries_RunId_UserbackFeedbackId",
                table: "UserbackImportLogEntries",
                columns: new[] { "RunId", "UserbackFeedbackId" });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_CardId_UserbackCommentId",
                table: "Comments",
                columns: new[] { "CardId", "UserbackCommentId" });

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_CardId_SourceUrlHash",
                table: "Attachments",
                columns: new[] { "CardId", "SourceUrlHash" });

            migrationBuilder.CreateIndex(
                name: "IX_UserbackProjectSyncStates_UserbackProjectId",
                table: "UserbackProjectSyncStates",
                column: "UserbackProjectId",
                unique: true);

            // Data backfill — CardTag.Source defaults to Manual (0), but every card/tag link that
            // exists today was written by the Userback importer, so leaving them Manual would make
            // the ownership column inert on exactly the rows the sync has to set-diff. Verified
            // read-only against 420_eform-angular-kanban-plugin: all 1551 CardTags rows (and all
            // 1551 CardTagVersions rows) hang off a card with Source = 1.
            migrationBuilder.Sql("UPDATE CardTags ct JOIN Cards c ON c.Id = ct.CardId SET ct.Source = 1 WHERE c.Source = 1;");
            migrationBuilder.Sql("UPDATE CardTagVersions ctv JOIN Cards c ON c.Id = ctv.CardId SET ctv.Source = 1 WHERE c.Source = 1;");

            // Data backfill — every run that predates this migration was a full import, and its
            // single CardsImported aggregate was a create count. Without this they render as
            // "Incremental, 0 cards created". The version rows are snapshots of those same runs,
            // so they get the same treatment (each keeps whatever CardsImported it captured).
            migrationBuilder.Sql("UPDATE UserbackImportRuns SET CardsCreated = CardsImported, Mode = 1;");
            migrationBuilder.Sql("UPDATE UserbackImportRunVersions SET CardsCreated = CardsImported, Mode = 1;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserbackProjectSyncStates");

            migrationBuilder.DropTable(
                name: "UserbackProjectSyncStateVersions");

            migrationBuilder.DropIndex(
                name: "IX_UserbackImportLogEntries_RunId_UserbackFeedbackId",
                table: "UserbackImportLogEntries");

            migrationBuilder.DropIndex(
                name: "IX_Comments_CardId_UserbackCommentId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Attachments_CardId_SourceUrlHash",
                table: "Attachments");

            migrationBuilder.DropColumn(
                name: "CardsCreated",
                table: "UserbackImportRunVersions");

            migrationBuilder.DropColumn(
                name: "CardsSkipped",
                table: "UserbackImportRunVersions");

            migrationBuilder.DropColumn(
                name: "CardsSoftDeleted",
                table: "UserbackImportRunVersions");

            migrationBuilder.DropColumn(
                name: "CardsUpdated",
                table: "UserbackImportRunVersions");

            migrationBuilder.DropColumn(
                name: "LastHeartbeatAt",
                table: "UserbackImportRunVersions");

            migrationBuilder.DropColumn(
                name: "MediaFailed",
                table: "UserbackImportRunVersions");

            migrationBuilder.DropColumn(
                name: "Mode",
                table: "UserbackImportRunVersions");

            migrationBuilder.DropColumn(
                name: "TotalToProcess",
                table: "UserbackImportRunVersions");

            migrationBuilder.DropColumn(
                name: "CardsCreated",
                table: "UserbackImportRuns");

            migrationBuilder.DropColumn(
                name: "CardsSkipped",
                table: "UserbackImportRuns");

            migrationBuilder.DropColumn(
                name: "CardsSoftDeleted",
                table: "UserbackImportRuns");

            migrationBuilder.DropColumn(
                name: "CardsUpdated",
                table: "UserbackImportRuns");

            migrationBuilder.DropColumn(
                name: "LastHeartbeatAt",
                table: "UserbackImportRuns");

            migrationBuilder.DropColumn(
                name: "MediaFailed",
                table: "UserbackImportRuns");

            migrationBuilder.DropColumn(
                name: "Mode",
                table: "UserbackImportRuns");

            migrationBuilder.DropColumn(
                name: "TotalToProcess",
                table: "UserbackImportRuns");

            migrationBuilder.DropColumn(
                name: "UserbackCommentId",
                table: "CommentVersions");

            migrationBuilder.DropColumn(
                name: "UserbackCommentId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "Source",
                table: "CardTagVersions");

            migrationBuilder.DropColumn(
                name: "Source",
                table: "CardTags");

            migrationBuilder.DropColumn(
                name: "SourceUrl",
                table: "AttachmentVersions");

            migrationBuilder.DropColumn(
                name: "SourceUrlHash",
                table: "AttachmentVersions");

            migrationBuilder.DropColumn(
                name: "SourceUrl",
                table: "Attachments");

            migrationBuilder.DropColumn(
                name: "SourceUrlHash",
                table: "Attachments");

            // Intentionally one-way: the unique IX_UserbackImportLogEntries_UserbackFeedbackId_Status
            // that Up() drops is NOT recreated here. Once the corrected importer has run, the log
            // legitimately holds repeated (UserbackFeedbackId, Status) pairs — removing that
            // constraint is the entire point of Up() — so recreating it would fail on real data.
            // MySQL DDL is non-transactional, so a Down() that dies partway would leave the database
            // reachable by neither Up() nor Down(). Roll back by restoring a backup instead.
        }
    }
}
