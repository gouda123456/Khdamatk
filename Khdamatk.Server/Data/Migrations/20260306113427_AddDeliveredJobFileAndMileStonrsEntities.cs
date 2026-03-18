using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Khdamatk.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddDeliveredJobFileAndMileStonrsEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            

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
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
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

            migrationBuilder.CreateTable(
                name: "DeliveredJobFile",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobId = table.Column<int>(type: "int", nullable: false),
                    MileStoneId = table.Column<int>(type: "int", nullable: false),
                    MediaId = table.Column<int>(type: "int", nullable: false),
                    Statues = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveredJobFile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeliveredJobFile_JobPosts_JobId",
                        column: x => x.JobId,
                        principalTable: "JobPosts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DeliveredJobFile_Medias_MediaId",
                        column: x => x.MediaId,
                        principalTable: "Medias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DeliveredJobFile_MileStones_MileStoneId",
                        column: x => x.MileStoneId,
                        principalTable: "MileStones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            

            migrationBuilder.CreateIndex(
                name: "IX_DeliveredJobFile_JobId",
                table: "DeliveredJobFile",
                column: "JobId");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveredJobFile_MediaId",
                table: "DeliveredJobFile",
                column: "MediaId");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveredJobFile_MileStoneId",
                table: "DeliveredJobFile",
                column: "MileStoneId");

            migrationBuilder.CreateIndex(
                name: "IX_MileStones_JobPostId",
                table: "MileStones",
                column: "JobPostId");

            migrationBuilder.CreateIndex(
                name: "IX_MileStones_Title",
                table: "MileStones",
                column: "Title");

            

            
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            

            migrationBuilder.DropTable(
                name: "DeliveredJobFile");

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

            
        }
    }
}
