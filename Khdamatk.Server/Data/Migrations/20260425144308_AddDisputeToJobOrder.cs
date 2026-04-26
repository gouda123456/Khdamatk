using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Khdamatk.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddDisputeToJobOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_ServiceOrders_ServiceOrderId1",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_ServiceOrderId1",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "ServiceOrderId1",
                table: "Reviews");

            migrationBuilder.AddColumn<int>(
                name: "ConversationId",
                table: "ServiceOrders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DisputeId",
                table: "ServiceOrders",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReviewId",
                table: "ServiceOrders",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ServiceOrders_ConversationId",
                table: "ServiceOrders",
                column: "ConversationId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceOrders_DisputeId",
                table: "ServiceOrders",
                column: "DisputeId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceOrders_ReviewId",
                table: "ServiceOrders",
                column: "ReviewId");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceOrders_Conversations_ConversationId",
                table: "ServiceOrders",
                column: "ConversationId",
                principalTable: "Conversations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceOrders_Disputes_DisputeId",
                table: "ServiceOrders",
                column: "DisputeId",
                principalTable: "Disputes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceOrders_Reviews_ReviewId",
                table: "ServiceOrders",
                column: "ReviewId",
                principalTable: "Reviews",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceOrders_Conversations_ConversationId",
                table: "ServiceOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceOrders_Disputes_DisputeId",
                table: "ServiceOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceOrders_Reviews_ReviewId",
                table: "ServiceOrders");

            migrationBuilder.DropIndex(
                name: "IX_ServiceOrders_ConversationId",
                table: "ServiceOrders");

            migrationBuilder.DropIndex(
                name: "IX_ServiceOrders_DisputeId",
                table: "ServiceOrders");

            migrationBuilder.DropIndex(
                name: "IX_ServiceOrders_ReviewId",
                table: "ServiceOrders");

            migrationBuilder.DropColumn(
                name: "ConversationId",
                table: "ServiceOrders");

            migrationBuilder.DropColumn(
                name: "DisputeId",
                table: "ServiceOrders");

            migrationBuilder.DropColumn(
                name: "ReviewId",
                table: "ServiceOrders");

            migrationBuilder.AddColumn<int>(
                name: "ServiceOrderId1",
                table: "Reviews",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_ServiceOrderId1",
                table: "Reviews",
                column: "ServiceOrderId1",
                unique: true,
                filter: "[ServiceOrderId1] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_ServiceOrders_ServiceOrderId1",
                table: "Reviews",
                column: "ServiceOrderId1",
                principalTable: "ServiceOrders",
                principalColumn: "Id");
        }
    }
}
