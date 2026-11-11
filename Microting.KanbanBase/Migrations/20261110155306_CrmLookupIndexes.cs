using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Microting.KanbanBase.Migrations
{
    /// <inheritdoc />
    public partial class CrmLookupIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_CardTenants_CustomerNo",
                table: "CardTenants",
                column: "CustomerNo");

            migrationBuilder.CreateIndex(
                name: "IX_Cards_ExternalSource_ExternalId",
                table: "Cards",
                columns: new[] { "ExternalSource", "ExternalId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CardTenants_CustomerNo",
                table: "CardTenants");

            migrationBuilder.DropIndex(
                name: "IX_Cards_ExternalSource_ExternalId",
                table: "Cards");
        }
    }
}
