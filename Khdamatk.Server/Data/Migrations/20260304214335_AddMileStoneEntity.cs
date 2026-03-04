using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Khdamatk.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddMileStoneEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_serviceOrders_Services_ServiceID",
                table: "serviceOrders");

            migrationBuilder.DropColumn(
                name: "DeliverTimeInDays",
                table: "Services");

            migrationBuilder.RenameColumn(
                name: "ServiceID",
                table: "serviceOrders",
                newName: "ServiceId");

            migrationBuilder.RenameIndex(
                name: "IX_serviceOrders_ServiceID",
                table: "serviceOrders",
                newName: "IX_serviceOrders_ServiceId");

            migrationBuilder.AddColumn<int>(
                name: "AverageResponseTime",
                table: "ServiceProviderProfiles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "ServiceId",
                table: "serviceOrders",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "ServiceID",
                table: "serviceOrders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "MileStones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    StepNumber = table.Column<int>(type: "int", nullable: false),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false),
                    JobPostId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MileStones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MileStones_JobPosts_JobPostId",
                        column: x => x.JobPostId,
                        principalTable: "JobPosts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_serviceOrders_ServiceID",
                table: "serviceOrders",
                column: "ServiceID");

            migrationBuilder.CreateIndex(
                name: "IX_MileStones_JobPostId",
                table: "MileStones",
                column: "JobPostId");

            migrationBuilder.CreateIndex(
                name: "IX_MileStones_Title",
                table: "MileStones",
                column: "Title");

            migrationBuilder.AddForeignKey(
                name: "FK_serviceOrders_Services_ServiceID",
                table: "serviceOrders",
                column: "ServiceID",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_serviceOrders_Services_ServiceId",
                table: "serviceOrders",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_serviceOrders_Services_ServiceID",
                table: "serviceOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_serviceOrders_Services_ServiceId",
                table: "serviceOrders");

            migrationBuilder.DropTable(
                name: "MileStones");

            migrationBuilder.DropIndex(
                name: "IX_serviceOrders_ServiceID",
                table: "serviceOrders");

            migrationBuilder.DropColumn(
                name: "AverageResponseTime",
                table: "ServiceProviderProfiles");

            migrationBuilder.DropColumn(
                name: "ServiceID",
                table: "serviceOrders");

            migrationBuilder.RenameColumn(
                name: "ServiceId",
                table: "serviceOrders",
                newName: "ServiceID");

            migrationBuilder.RenameIndex(
                name: "IX_serviceOrders_ServiceId",
                table: "serviceOrders",
                newName: "IX_serviceOrders_ServiceID");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "DeliverTimeInDays",
                table: "Services",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AlterColumn<int>(
                name: "ServiceID",
                table: "serviceOrders",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_serviceOrders_Services_ServiceID",
                table: "serviceOrders",
                column: "ServiceID",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
