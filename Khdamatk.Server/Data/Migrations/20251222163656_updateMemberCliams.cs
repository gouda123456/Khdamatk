using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Khdamatk.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class updateMemberCliams : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoleClaims",
                columns: new[] { "Id", "ClaimType", "ClaimValue", "RoleId" },
                values: new object[,]
                {
                    { 7, "permissions", "WeatherForecast:View", "e3b0c442-98fc-4c2a-8b2e-3f9d6f1a1a66" },
                    { 8, "permissions", "Authentications:View", "e3b0c442-98fc-4c2a-8b2e-3f9d6f1a1a66" },
                    { 9, "permissions", "Users:View", "e3b0c442-98fc-4c2a-8b2e-3f9d6f1a1a66" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 9);
        }
    }
}
