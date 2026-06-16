using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Khdamatk.Server.Data.migrations
{
    /// <inheritdoc />
    public partial class AddKycColumnsOnly : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IdBackUrl",
                table: "VerificationData",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "IdFrontUrl",
                table: "VerificationData",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RejectNotes",
                table: "VerificationData",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SelfieWithIdUrl",
                table: "VerificationData",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdBackUrl",
                table: "VerificationData");

            migrationBuilder.DropColumn(
                name: "IdFrontUrl",
                table: "VerificationData");

            migrationBuilder.DropColumn(
                name: "RejectNotes",
                table: "VerificationData");

            migrationBuilder.DropColumn(
                name: "SelfieWithIdUrl",
                table: "VerificationData");
        }
    }
}
