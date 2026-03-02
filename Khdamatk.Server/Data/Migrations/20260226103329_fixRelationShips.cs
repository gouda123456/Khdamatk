using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Khdamatk.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class fixRelationShips : Migration
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
                principalColumn: "Id");

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
        }
    }
}
