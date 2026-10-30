using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Microting.KanbanBase.Migrations
{
    /// <inheritdoc />
    public partial class CommentExternalSource : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ExternalSource",
                table: "CommentVersions",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "ExternalSource",
                table: "Comments",
                type: "varchar(32)",
                maxLength: 32,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExternalSource",
                table: "CommentVersions");

            migrationBuilder.DropColumn(
                name: "ExternalSource",
                table: "Comments");
        }
    }
}
