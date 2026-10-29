using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Microting.KanbanBase.Migrations
{
    /// <inheritdoc />
    public partial class CardAttention : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Attention",
                table: "CardVersions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "AttentionChangedAt",
                table: "CardVersions",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AttentionNote",
                table: "CardVersions",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "Attention",
                table: "Cards",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "AttentionChangedAt",
                table: "Cards",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AttentionNote",
                table: "Cards",
                type: "varchar(200)",
                maxLength: 200,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Attention",
                table: "CardVersions");

            migrationBuilder.DropColumn(
                name: "AttentionChangedAt",
                table: "CardVersions");

            migrationBuilder.DropColumn(
                name: "AttentionNote",
                table: "CardVersions");

            migrationBuilder.DropColumn(
                name: "Attention",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "AttentionChangedAt",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "AttentionNote",
                table: "Cards");
        }
    }
}
