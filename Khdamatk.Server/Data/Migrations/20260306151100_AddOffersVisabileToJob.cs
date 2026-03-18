using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Khdamatk.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddOffersVisabileToJob : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_serviceOrders_ServiceOrderId",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_ServiceOrderId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "ServiceOrderId",
                table: "Messages");

            migrationBuilder.AddColumn<int>(
                name: "ServiceOrderId1",
                table: "Conversations",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Conversations_ServiceOrderId1",
                table: "Conversations",
                column: "ServiceOrderId1",
                unique: true,
                filter: "[ServiceOrderId1] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Conversations_serviceOrders_ServiceOrderId1",
                table: "Conversations",
                column: "ServiceOrderId1",
                principalTable: "serviceOrders",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Conversations_serviceOrders_ServiceOrderId1",
                table: "Conversations");

            migrationBuilder.DropIndex(
                name: "IX_Conversations_ServiceOrderId1",
                table: "Conversations");

            migrationBuilder.DropColumn(
                name: "ServiceOrderId1",
                table: "Conversations");

            migrationBuilder.AddColumn<int>(
                name: "ServiceOrderId",
                table: "Messages",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ServiceOrderId",
                table: "Messages",
                column: "ServiceOrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_serviceOrders_ServiceOrderId",
                table: "Messages",
                column: "ServiceOrderId",
                principalTable: "serviceOrders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
