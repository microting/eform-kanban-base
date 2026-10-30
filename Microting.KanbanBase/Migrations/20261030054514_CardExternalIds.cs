using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Microting.KanbanBase.Migrations
{
    /// <inheritdoc />
    public partial class CardExternalIds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ExternalId",
                table: "CardVersions",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "ExternalSource",
                table: "CardVersions",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "ExternalThreadId",
                table: "CardVersions",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "ExternalId",
                table: "Cards",
                type: "varchar(128)",
                maxLength: 128,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "ExternalSource",
                table: "Cards",
                type: "varchar(32)",
                maxLength: 32,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "ExternalThreadId",
                table: "Cards",
                type: "varchar(128)",
                maxLength: 128,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Cards_BoardId_ExternalSource_ExternalId",
                table: "Cards",
                columns: new[] { "BoardId", "ExternalSource", "ExternalId" });

            migrationBuilder.CreateIndex(
                name: "IX_Cards_BoardId_ExternalSource_ExternalThreadId",
                table: "Cards",
                columns: new[] { "BoardId", "ExternalSource", "ExternalThreadId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Cards_BoardId_ExternalSource_ExternalId",
                table: "Cards");

            migrationBuilder.DropIndex(
                name: "IX_Cards_BoardId_ExternalSource_ExternalThreadId",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "ExternalId",
                table: "CardVersions");

            migrationBuilder.DropColumn(
                name: "ExternalSource",
                table: "CardVersions");

            migrationBuilder.DropColumn(
                name: "ExternalThreadId",
                table: "CardVersions");

            migrationBuilder.DropColumn(
                name: "ExternalId",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "ExternalSource",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "ExternalThreadId",
                table: "Cards");
        }
    }
}
