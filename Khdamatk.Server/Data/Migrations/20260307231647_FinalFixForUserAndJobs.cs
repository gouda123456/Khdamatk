using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Khdamatk.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class FinalFixForUserAndJobs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Medias_ProfilePictureId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_ServiceProviderProfiles_ServiceProviderProfileUserId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Certificates_ServiceProviderProfiles_ServiceProviderProfileId",
                table: "Certificates");

            migrationBuilder.DropForeignKey(
                name: "FK_Conversations_serviceOrders_ServiceOrderId",
                table: "Conversations");

            migrationBuilder.DropForeignKey(
                name: "FK_JobSkillRequirements_JobPosts_JobPostId",
                table: "JobSkillRequirements");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Conversations_ConversationId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_PaymentTransactions_CreditCards_CreditCardId",
                table: "PaymentTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_ProviderSkills_ServiceProviderProfiles_ServiceProviderProfileId",
                table: "ProviderSkills");

            migrationBuilder.DropForeignKey(
                name: "FK_RefreshTokens_AspNetUsers_UserId",
                table: "RefreshTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_ServiceProviderProfiles_ServiceProviderId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_serviceOrders_OrderId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceProviderProfiles_AspNetUsers_UserId",
                table: "ServiceProviderProfiles");

            migrationBuilder.DropForeignKey(
                name: "FK_Services_Medias_MainMediaId",
                table: "Services");

            migrationBuilder.DropForeignKey(
                name: "FK_Services_ServiceProviderProfiles_ServiceProviderProfileId",
                table: "Services");

            migrationBuilder.DropForeignKey(
                name: "FK_UserFavorites_AspNetUsers_UserId",
                table: "UserFavorites");

            migrationBuilder.DropForeignKey(
                name: "FK_VerificationsCodes_AspNetUsers_UserId",
                table: "VerificationsCodes");

            migrationBuilder.AddColumn<int>(
                name: "invoiceId",
                table: "serviceOrders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "invoiceKey",
                table: "serviceOrders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "AspNetUsers",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Jobs",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Budget = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jobs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Jobs_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Reports",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    JobId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClientName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FreelancerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AdminComment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompensationAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ReviewedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reports", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReportAttachments",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReportId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportAttachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReportAttachments_Reports_ReportId",
                        column: x => x.ReportId,
                        principalTable: "Reports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ReportMessages",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SenderName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsAdmin = table.Column<bool>(type: "bit", nullable: false),
                    ReportId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReportMessages_Reports_ReportId",
                        column: x => x.ReportId,
                        principalTable: "Reports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b74ddd14-6340-4840-95c2-db12554843e5",
                columns: new[] { "CreatedAt", "FullName", "Role", "Status" },
                values: new object[] { new DateTime(2026, 3, 7, 23, 16, 45, 864, DateTimeKind.Utc).AddTicks(4899), "", "User", "Active" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b74ddd14-6340-4840-95c2-db12554843eslkna5",
                columns: new[] { "CreatedAt", "FullName", "Role", "Status" },
                values: new object[] { new DateTime(2026, 3, 7, 23, 16, 45, 865, DateTimeKind.Utc).AddTicks(5414), "", "User", "Active" });

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_UserId",
                table: "Jobs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportAttachments_ReportId",
                table: "ReportAttachments",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportMessages_ReportId",
                table: "ReportMessages",
                column: "ReportId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Medias_ProfilePictureId",
                table: "AspNetUsers",
                column: "ProfilePictureId",
                principalTable: "Medias",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_ServiceProviderProfiles_ServiceProviderProfileUserId",
                table: "AspNetUsers",
                column: "ServiceProviderProfileUserId",
                principalTable: "ServiceProviderProfiles",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Certificates_ServiceProviderProfiles_ServiceProviderProfileId",
                table: "Certificates",
                column: "ServiceProviderProfileId",
                principalTable: "ServiceProviderProfiles",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Conversations_serviceOrders_ServiceOrderId",
                table: "Conversations",
                column: "ServiceOrderId",
                principalTable: "serviceOrders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_JobSkillRequirements_JobPosts_JobPostId",
                table: "JobSkillRequirements",
                column: "JobPostId",
                principalTable: "JobPosts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Conversations_ConversationId",
                table: "Messages",
                column: "ConversationId",
                principalTable: "Conversations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentTransactions_CreditCards_CreditCardId",
                table: "PaymentTransactions",
                column: "CreditCardId",
                principalTable: "CreditCards",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProviderSkills_ServiceProviderProfiles_ServiceProviderProfileId",
                table: "ProviderSkills",
                column: "ServiceProviderProfileId",
                principalTable: "ServiceProviderProfiles",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshTokens_AspNetUsers_UserId",
                table: "RefreshTokens",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_ServiceProviderProfiles_ServiceProviderId",
                table: "Reviews",
                column: "ServiceProviderId",
                principalTable: "ServiceProviderProfiles",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_serviceOrders_OrderId",
                table: "Reviews",
                column: "OrderId",
                principalTable: "serviceOrders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceProviderProfiles_AspNetUsers_UserId",
                table: "ServiceProviderProfiles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Services_Medias_MainMediaId",
                table: "Services",
                column: "MainMediaId",
                principalTable: "Medias",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Services_ServiceProviderProfiles_ServiceProviderProfileId",
                table: "Services",
                column: "ServiceProviderProfileId",
                principalTable: "ServiceProviderProfiles",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserFavorites_AspNetUsers_UserId",
                table: "UserFavorites",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VerificationsCodes_AspNetUsers_UserId",
                table: "VerificationsCodes",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Medias_ProfilePictureId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_ServiceProviderProfiles_ServiceProviderProfileUserId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Certificates_ServiceProviderProfiles_ServiceProviderProfileId",
                table: "Certificates");

            migrationBuilder.DropForeignKey(
                name: "FK_Conversations_serviceOrders_ServiceOrderId",
                table: "Conversations");

            migrationBuilder.DropForeignKey(
                name: "FK_JobSkillRequirements_JobPosts_JobPostId",
                table: "JobSkillRequirements");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Conversations_ConversationId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_PaymentTransactions_CreditCards_CreditCardId",
                table: "PaymentTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_ProviderSkills_ServiceProviderProfiles_ServiceProviderProfileId",
                table: "ProviderSkills");

            migrationBuilder.DropForeignKey(
                name: "FK_RefreshTokens_AspNetUsers_UserId",
                table: "RefreshTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_ServiceProviderProfiles_ServiceProviderId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_serviceOrders_OrderId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceProviderProfiles_AspNetUsers_UserId",
                table: "ServiceProviderProfiles");

            migrationBuilder.DropForeignKey(
                name: "FK_Services_Medias_MainMediaId",
                table: "Services");

            migrationBuilder.DropForeignKey(
                name: "FK_Services_ServiceProviderProfiles_ServiceProviderProfileId",
                table: "Services");

            migrationBuilder.DropForeignKey(
                name: "FK_UserFavorites_AspNetUsers_UserId",
                table: "UserFavorites");

            migrationBuilder.DropForeignKey(
                name: "FK_VerificationsCodes_AspNetUsers_UserId",
                table: "VerificationsCodes");

            migrationBuilder.DropTable(
                name: "Jobs");

            migrationBuilder.DropTable(
                name: "ReportAttachments");

            migrationBuilder.DropTable(
                name: "ReportMessages");

            migrationBuilder.DropTable(
                name: "Reports");

            migrationBuilder.DropColumn(
                name: "invoiceId",
                table: "serviceOrders");

            migrationBuilder.DropColumn(
                name: "invoiceKey",
                table: "serviceOrders");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FullName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "AspNetUsers",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Medias_ProfilePictureId",
                table: "AspNetUsers",
                column: "ProfilePictureId",
                principalTable: "Medias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_ServiceProviderProfiles_ServiceProviderProfileUserId",
                table: "AspNetUsers",
                column: "ServiceProviderProfileUserId",
                principalTable: "ServiceProviderProfiles",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Certificates_ServiceProviderProfiles_ServiceProviderProfileId",
                table: "Certificates",
                column: "ServiceProviderProfileId",
                principalTable: "ServiceProviderProfiles",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Conversations_serviceOrders_ServiceOrderId",
                table: "Conversations",
                column: "ServiceOrderId",
                principalTable: "serviceOrders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_JobSkillRequirements_JobPosts_JobPostId",
                table: "JobSkillRequirements",
                column: "JobPostId",
                principalTable: "JobPosts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Conversations_ConversationId",
                table: "Messages",
                column: "ConversationId",
                principalTable: "Conversations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentTransactions_CreditCards_CreditCardId",
                table: "PaymentTransactions",
                column: "CreditCardId",
                principalTable: "CreditCards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProviderSkills_ServiceProviderProfiles_ServiceProviderProfileId",
                table: "ProviderSkills",
                column: "ServiceProviderProfileId",
                principalTable: "ServiceProviderProfiles",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshTokens_AspNetUsers_UserId",
                table: "RefreshTokens",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_ServiceProviderProfiles_ServiceProviderId",
                table: "Reviews",
                column: "ServiceProviderId",
                principalTable: "ServiceProviderProfiles",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_serviceOrders_OrderId",
                table: "Reviews",
                column: "OrderId",
                principalTable: "serviceOrders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceProviderProfiles_AspNetUsers_UserId",
                table: "ServiceProviderProfiles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Services_Medias_MainMediaId",
                table: "Services",
                column: "MainMediaId",
                principalTable: "Medias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Services_ServiceProviderProfiles_ServiceProviderProfileId",
                table: "Services",
                column: "ServiceProviderProfileId",
                principalTable: "ServiceProviderProfiles",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserFavorites_AspNetUsers_UserId",
                table: "UserFavorites",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VerificationsCodes_AspNetUsers_UserId",
                table: "VerificationsCodes",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
