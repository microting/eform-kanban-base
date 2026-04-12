using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace Microting.KanbanBase.Migrations
{
    public partial class MultiRepoGitHub : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Create GitHubAppSettings table
            migrationBuilder.CreateTable(
                name: "GitHubAppSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AppId = table.Column<long>(type: "bigint", nullable: false),
                    AppName = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    PrivateKeyEncrypted = table.Column<string>(type: "longtext", nullable: false),
                    PrivateKeyIv = table.Column<string>(type: "longtext", nullable: false),
                    ClientId = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    ClientSecretEncrypted = table.Column<string>(type: "longtext", nullable: true),
                    ClientSecretIv = table.Column<string>(type: "longtext", nullable: true),
                    WebhookSecret = table.Column<string>(type: "longtext", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    WorkflowState = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: false),
                    UpdatedByUserId = table.Column<int>(type: "int", nullable: false),
                    Version = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GitHubAppSettings", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            // Create GitHubAppSettingVersions table
            migrationBuilder.CreateTable(
                name: "GitHubAppSettingVersions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    GitHubAppSettingId = table.Column<int>(type: "int", nullable: false),
                    AppId = table.Column<long>(type: "bigint", nullable: false),
                    AppName = table.Column<string>(type: "longtext", nullable: false),
                    PrivateKeyEncrypted = table.Column<string>(type: "longtext", nullable: false),
                    PrivateKeyIv = table.Column<string>(type: "longtext", nullable: false),
                    ClientId = table.Column<string>(type: "longtext", nullable: false),
                    ClientSecretEncrypted = table.Column<string>(type: "longtext", nullable: true),
                    ClientSecretIv = table.Column<string>(type: "longtext", nullable: true),
                    WebhookSecret = table.Column<string>(type: "longtext", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    WorkflowState = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: false),
                    UpdatedByUserId = table.Column<int>(type: "int", nullable: false),
                    Version = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GitHubAppSettingVersions", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            // Create ProjectRepositories table
            migrationBuilder.CreateTable(
                name: "ProjectRepositories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    Owner = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    Repo = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    Source = table.Column<int>(type: "int", nullable: false),
                    GitHubPatEncrypted = table.Column<string>(type: "longtext", nullable: true),
                    GitHubPatIv = table.Column<string>(type: "longtext", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    WorkflowState = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: false),
                    UpdatedByUserId = table.Column<int>(type: "int", nullable: false),
                    Version = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectRepositories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectRepositories_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            // Create ProjectRepositoryVersions table
            migrationBuilder.CreateTable(
                name: "ProjectRepositoryVersions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ProjectRepositoryId = table.Column<int>(type: "int", nullable: false),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    Owner = table.Column<string>(type: "longtext", nullable: false),
                    Repo = table.Column<string>(type: "longtext", nullable: false),
                    Source = table.Column<int>(type: "int", nullable: false),
                    GitHubPatEncrypted = table.Column<string>(type: "longtext", nullable: true),
                    GitHubPatIv = table.Column<string>(type: "longtext", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    WorkflowState = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: false),
                    UpdatedByUserId = table.Column<int>(type: "int", nullable: false),
                    Version = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectRepositoryVersions", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            // Create CardGitHubLinks table
            migrationBuilder.CreateTable(
                name: "CardGitHubLinks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CardId = table.Column<int>(type: "int", nullable: false),
                    ProjectRepositoryId = table.Column<int>(type: "int", nullable: false),
                    IssueNumber = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    WorkflowState = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: false),
                    UpdatedByUserId = table.Column<int>(type: "int", nullable: false),
                    Version = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardGitHubLinks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CardGitHubLinks_Cards_CardId",
                        column: x => x.CardId,
                        principalTable: "Cards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CardGitHubLinks_ProjectRepositories_ProjectRepositoryId",
                        column: x => x.ProjectRepositoryId,
                        principalTable: "ProjectRepositories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            // Create CardGitHubLinkVersions table
            migrationBuilder.CreateTable(
                name: "CardGitHubLinkVersions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CardGitHubLinkId = table.Column<int>(type: "int", nullable: false),
                    CardId = table.Column<int>(type: "int", nullable: false),
                    ProjectRepositoryId = table.Column<int>(type: "int", nullable: false),
                    IssueNumber = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    WorkflowState = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: false),
                    UpdatedByUserId = table.Column<int>(type: "int", nullable: false),
                    Version = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardGitHubLinkVersions", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            // Add GitHubAppInstallationId to Projects
            migrationBuilder.AddColumn<long>(
                name: "GitHubAppInstallationId",
                table: "Projects",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "GitHubAppInstallationId",
                table: "ProjectVersions",
                type: "bigint",
                nullable: true);

            // Create indexes
            migrationBuilder.CreateIndex(
                name: "IX_ProjectRepositories_ProjectId_Owner_Repo",
                table: "ProjectRepositories",
                columns: new[] { "ProjectId", "Owner", "Repo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CardGitHubLinks_CardId_ProjectRepositoryId_IssueNumber",
                table: "CardGitHubLinks",
                columns: new[] { "CardId", "ProjectRepositoryId", "IssueNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CardGitHubLinks_ProjectRepositoryId",
                table: "CardGitHubLinks",
                column: "ProjectRepositoryId");

            // Drop old GitHub columns from Projects
            migrationBuilder.DropColumn(name: "GitHubOwner", table: "Projects");
            migrationBuilder.DropColumn(name: "GitHubRepo", table: "Projects");
            migrationBuilder.DropColumn(name: "GitHubPatEncrypted", table: "Projects");
            migrationBuilder.DropColumn(name: "GitHubPatIv", table: "Projects");
            migrationBuilder.DropColumn(name: "GitHubOwner", table: "ProjectVersions");
            migrationBuilder.DropColumn(name: "GitHubRepo", table: "ProjectVersions");
            migrationBuilder.DropColumn(name: "GitHubPatEncrypted", table: "ProjectVersions");
            migrationBuilder.DropColumn(name: "GitHubPatIv", table: "ProjectVersions");

            // Drop GitHubIssueNumber from Cards
            migrationBuilder.DropColumn(name: "GitHubIssueNumber", table: "Cards");
            migrationBuilder.DropColumn(name: "GitHubIssueNumber", table: "CardVersions");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Restore old columns
            migrationBuilder.AddColumn<string>(name: "GitHubOwner", table: "Projects", type: "varchar(200)", maxLength: 200, nullable: true);
            migrationBuilder.AddColumn<string>(name: "GitHubRepo", table: "Projects", type: "varchar(200)", maxLength: 200, nullable: true);
            migrationBuilder.AddColumn<string>(name: "GitHubPatEncrypted", table: "Projects", type: "longtext", nullable: true);
            migrationBuilder.AddColumn<string>(name: "GitHubPatIv", table: "Projects", type: "longtext", nullable: true);
            migrationBuilder.AddColumn<string>(name: "GitHubOwner", table: "ProjectVersions", type: "longtext", nullable: true);
            migrationBuilder.AddColumn<string>(name: "GitHubRepo", table: "ProjectVersions", type: "longtext", nullable: true);
            migrationBuilder.AddColumn<string>(name: "GitHubPatEncrypted", table: "ProjectVersions", type: "longtext", nullable: true);
            migrationBuilder.AddColumn<string>(name: "GitHubPatIv", table: "ProjectVersions", type: "longtext", nullable: true);
            migrationBuilder.AddColumn<int>(name: "GitHubIssueNumber", table: "Cards", type: "int", nullable: true);
            migrationBuilder.AddColumn<int>(name: "GitHubIssueNumber", table: "CardVersions", type: "int", nullable: true);

            // Drop new columns
            migrationBuilder.DropColumn(name: "GitHubAppInstallationId", table: "Projects");
            migrationBuilder.DropColumn(name: "GitHubAppInstallationId", table: "ProjectVersions");

            // Drop new tables
            migrationBuilder.DropTable(name: "CardGitHubLinkVersions");
            migrationBuilder.DropTable(name: "CardGitHubLinks");
            migrationBuilder.DropTable(name: "ProjectRepositoryVersions");
            migrationBuilder.DropTable(name: "ProjectRepositories");
            migrationBuilder.DropTable(name: "GitHubAppSettingVersions");
            migrationBuilder.DropTable(name: "GitHubAppSettings");
        }
    }
}
