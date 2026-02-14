using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Khdamatk.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class addJobPostJobOfferAndEnchineConversionAndMakeitCenerlizedByContextPatternAndCoupling : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Bio",
                table: "ServiceProviderProfiles",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AddColumn<int>(
                name: "CompletedJobs",
                table: "ServiceProviderProfiles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsAvailable",
                table: "ServiceProviderProfiles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "FileExtension",
                table: "Medias",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "StoredFileName",
                table: "Medias",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ContextType",
                table: "Conversations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RelatedEntityId",
                table: "Conversations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ServiceProviderProfileUserId",
                table: "AspNetUsers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "JobPosts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BudgetMin = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BudgetMax = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Deadline = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobPosts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobPosts_AspNetUsers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_JobPosts_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "jobOffers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CoverLetter = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProposedPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DeliveryTimeInDays = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    JobPostId = table.Column<int>(type: "int", nullable: false),
                    ProviderProfileId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ConversationId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_jobOffers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_jobOffers_Conversations_ConversationId",
                        column: x => x.ConversationId,
                        principalTable: "Conversations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_jobOffers_JobPosts_JobPostId",
                        column: x => x.JobPostId,
                        principalTable: "JobPosts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_jobOffers_ServiceProviderProfiles_ProviderProfileId",
                        column: x => x.ProviderProfileId,
                        principalTable: "ServiceProviderProfiles",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b74ddd14-6340-4840-95c2-db12554843e5",
                column: "ServiceProviderProfileUserId",
                value: null);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b74ddd14-6340-4840-95c2-db12554843eslkna5",
                column: "ServiceProviderProfileUserId",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_ServiceProviderProfiles_IsActive",
                table: "ServiceProviderProfiles",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceProviderProfiles_IsAvailable",
                table: "ServiceProviderProfiles",
                column: "IsAvailable");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ServiceProviderProfileUserId",
                table: "AspNetUsers",
                column: "ServiceProviderProfileUserId");

            migrationBuilder.CreateIndex(
                name: "IX_jobOffers_ConversationId",
                table: "jobOffers",
                column: "ConversationId");

            migrationBuilder.CreateIndex(
                name: "IX_jobOffers_JobPostId",
                table: "jobOffers",
                column: "JobPostId");

            migrationBuilder.CreateIndex(
                name: "IX_jobOffers_ProviderProfileId",
                table: "jobOffers",
                column: "ProviderProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_JobPosts_CategoryId",
                table: "JobPosts",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_JobPosts_CustomerId",
                table: "JobPosts",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_ServiceProviderProfiles_ServiceProviderProfileUserId",
                table: "AspNetUsers",
                column: "ServiceProviderProfileUserId",
                principalTable: "ServiceProviderProfiles",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_ServiceProviderProfiles_ServiceProviderProfileUserId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "jobOffers");

            migrationBuilder.DropTable(
                name: "JobPosts");

            migrationBuilder.DropIndex(
                name: "IX_ServiceProviderProfiles_IsActive",
                table: "ServiceProviderProfiles");

            migrationBuilder.DropIndex(
                name: "IX_ServiceProviderProfiles_IsAvailable",
                table: "ServiceProviderProfiles");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ServiceProviderProfileUserId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CompletedJobs",
                table: "ServiceProviderProfiles");

            migrationBuilder.DropColumn(
                name: "IsAvailable",
                table: "ServiceProviderProfiles");

            migrationBuilder.DropColumn(
                name: "FileExtension",
                table: "Medias");

            migrationBuilder.DropColumn(
                name: "StoredFileName",
                table: "Medias");

            migrationBuilder.DropColumn(
                name: "ContextType",
                table: "Conversations");

            migrationBuilder.DropColumn(
                name: "RelatedEntityId",
                table: "Conversations");

            migrationBuilder.DropColumn(
                name: "ServiceProviderProfileUserId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "Bio",
                table: "ServiceProviderProfiles",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000);
        }
    }
}
