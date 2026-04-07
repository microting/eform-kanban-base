using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Microting.KanbanBase.Migrations
{
    public partial class AddAttachmentMetadata : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var tables = new[] { "Attachments", "AttachmentVersions" };
            foreach (var table in tables)
            {
                migrationBuilder.AddColumn<string>(name: "Source", table: table, type: "longtext", nullable: true);
                migrationBuilder.AddColumn<string>(name: "PageUrl", table: table, type: "longtext", nullable: true);
                migrationBuilder.AddColumn<string>(name: "SystemInfo", table: table, type: "longtext", nullable: true);
                migrationBuilder.AddColumn<string>(name: "BrowserInfo", table: table, type: "longtext", nullable: true);
                migrationBuilder.AddColumn<int>(name: "ScreenWidth", table: table, type: "int", nullable: true);
                migrationBuilder.AddColumn<int>(name: "ScreenHeight", table: table, type: "int", nullable: true);
                migrationBuilder.AddColumn<int>(name: "BrowserWindowWidth", table: table, type: "int", nullable: true);
                migrationBuilder.AddColumn<int>(name: "BrowserWindowHeight", table: table, type: "int", nullable: true);
                migrationBuilder.AddColumn<double>(name: "DevicePixelRatio", table: table, type: "double", nullable: true);
                migrationBuilder.AddColumn<int>(name: "ColorDepth", table: table, type: "int", nullable: true);
                migrationBuilder.AddColumn<string>(name: "Location", table: table, type: "longtext", nullable: true);
                migrationBuilder.AddColumn<int>(name: "VideoDurationMs", table: table, type: "int", nullable: true);
            }
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var tables = new[] { "Attachments", "AttachmentVersions" };
            var columns = new[] { "Source", "PageUrl", "SystemInfo", "BrowserInfo", "ScreenWidth", "ScreenHeight",
                "BrowserWindowWidth", "BrowserWindowHeight", "DevicePixelRatio", "ColorDepth", "Location", "VideoDurationMs" };
            foreach (var table in tables)
                foreach (var col in columns)
                    migrationBuilder.DropColumn(name: col, table: table);
        }
    }
}
