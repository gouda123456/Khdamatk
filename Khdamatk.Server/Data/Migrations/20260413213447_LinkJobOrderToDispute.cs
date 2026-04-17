using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Khdamatk.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class LinkJobOrderToDispute : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Conversations_JobOrders_JobOrderId1",
                table: "Conversations");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_JobOrders_JobOrderId1",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_JobOrderId1",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Conversations_JobOrderId1",
                table: "Conversations");

            migrationBuilder.DropColumn(
                name: "JobOrderId1",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "JobOrderId1",
                table: "Conversations");

            migrationBuilder.AddColumn<int>(
                name: "ConversationId",
                table: "JobOrders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DisputeId",
                table: "JobOrders",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReviewId",
                table: "JobOrders",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_JobOrders_ConversationId",
                table: "JobOrders",
                column: "ConversationId");

            migrationBuilder.CreateIndex(
                name: "IX_JobOrders_DisputeId",
                table: "JobOrders",
                column: "DisputeId");

            migrationBuilder.CreateIndex(
                name: "IX_JobOrders_ReviewId",
                table: "JobOrders",
                column: "ReviewId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobOrders_Conversations_ConversationId",
                table: "JobOrders",
                column: "ConversationId",
                principalTable: "Conversations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_JobOrders_Disputes_DisputeId",
                table: "JobOrders",
                column: "DisputeId",
                principalTable: "Disputes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_JobOrders_Reviews_ReviewId",
                table: "JobOrders",
                column: "ReviewId",
                principalTable: "Reviews",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobOrders_Conversations_ConversationId",
                table: "JobOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_JobOrders_Disputes_DisputeId",
                table: "JobOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_JobOrders_Reviews_ReviewId",
                table: "JobOrders");

            migrationBuilder.DropIndex(
                name: "IX_JobOrders_ConversationId",
                table: "JobOrders");

            migrationBuilder.DropIndex(
                name: "IX_JobOrders_DisputeId",
                table: "JobOrders");

            migrationBuilder.DropIndex(
                name: "IX_JobOrders_ReviewId",
                table: "JobOrders");

            migrationBuilder.DropColumn(
                name: "ConversationId",
                table: "JobOrders");

            migrationBuilder.DropColumn(
                name: "DisputeId",
                table: "JobOrders");

            migrationBuilder.DropColumn(
                name: "ReviewId",
                table: "JobOrders");

            migrationBuilder.AddColumn<int>(
                name: "JobOrderId1",
                table: "Reviews",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "JobOrderId1",
                table: "Conversations",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_JobOrderId1",
                table: "Reviews",
                column: "JobOrderId1",
                unique: true,
                filter: "[JobOrderId1] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Conversations_JobOrderId1",
                table: "Conversations",
                column: "JobOrderId1",
                unique: true,
                filter: "[JobOrderId1] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Conversations_JobOrders_JobOrderId1",
                table: "Conversations",
                column: "JobOrderId1",
                principalTable: "JobOrders",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_JobOrders_JobOrderId1",
                table: "Reviews",
                column: "JobOrderId1",
                principalTable: "JobOrders",
                principalColumn: "Id");
        }
    }
}
