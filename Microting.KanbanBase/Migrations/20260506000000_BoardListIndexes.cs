using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Microting.KanbanBase.Migrations
{
    /// <inheritdoc />
    public partial class BoardListIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Comments_CardId_WorkflowState",
                table: "Comments",
                columns: new[] { "CardId", "WorkflowState" });

            migrationBuilder.CreateIndex(
                name: "IX_Cards_BoardId_ColumnId_WorkflowState_Position",
                table: "Cards",
                columns: new[] { "BoardId", "ColumnId", "WorkflowState", "Position" });

            migrationBuilder.CreateIndex(
                name: "IX_CardGitHubLinks_CardId_WorkflowState",
                table: "CardGitHubLinks",
                columns: new[] { "CardId", "WorkflowState" });

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_CardId_WorkflowState",
                table: "Attachments",
                columns: new[] { "CardId", "WorkflowState" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Comments_CardId_WorkflowState",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Cards_BoardId_ColumnId_WorkflowState_Position",
                table: "Cards");

            migrationBuilder.DropIndex(
                name: "IX_CardGitHubLinks_CardId_WorkflowState",
                table: "CardGitHubLinks");

            migrationBuilder.DropIndex(
                name: "IX_Attachments_CardId_WorkflowState",
                table: "Attachments");
        }
    }
}
