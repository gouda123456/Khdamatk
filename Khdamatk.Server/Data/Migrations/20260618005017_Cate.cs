using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Khdamatk.Server.Migrations
{
    /// <inheritdoc />
    public partial class Cate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Attachments",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "ContactEmail",
                table: "Categories",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "EmailNotifications",
                table: "Categories",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Icon",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Categories",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "MaintenanceMode",
                table: "Categories",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "PasswordPolicyEnabled",
                table: "Categories",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ServiceCount",
                table: "Categories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "SiteName",
                table: "Categories",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SmsNotifications",
                table: "Categories",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "TwoFactorAuthentication",
                table: "Categories",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "TwoFactorEnabled",
                table: "Categories",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Attachments",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "ContactEmail",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "EmailNotifications",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "Icon",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "MaintenanceMode",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "PasswordPolicyEnabled",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "ServiceCount",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "SiteName",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "SmsNotifications",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "TwoFactorAuthentication",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "TwoFactorEnabled",
                table: "Categories");
        }
    }
}
