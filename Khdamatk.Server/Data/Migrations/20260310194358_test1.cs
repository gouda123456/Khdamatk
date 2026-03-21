using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Khdamatk.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class test1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b74ddd14-6340-4840-95c2-db12554843e5");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b74ddd14-6340-4840-95c2-db12554843eslkna5");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "CreatedAt", "DateOfBirth", "Email", "EmailConfirmed", "FullName", "IsTrustedByAdmin", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "ProfilePictureId", "Role", "SecurityStamp", "ServiceProviderProfileUserId", "Status", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "b74ddd14-6340-4840-95c2-db12554843e5", 0, "abd61835-2981-4f28-9a87-3992de5a2460", new DateTime(2026, 3, 9, 19, 55, 49, 868, DateTimeKind.Utc).AddTicks(178), new DateTime(1990, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@admin.com", true, "", false, false, null, "ADMIN@ADMIN.COM", "ADMIN", "AQAAAAIAAYagAAAAEDTQ2MJwg8G7sNyX4vUuSzK8tnhxhzDsZpWvc8jt01mVjPURiVuM4G80qBh84/zwgA==", null, false, null, "User", "AQAAAAEAACcQAAAAEPG6fy3Z6k0rG+1bF5uV1r+H1l5H3g==", null, "Active", false, "Admin" },
                    { "b74ddd14-6340-4840-95c2-db12554843eslkna5", 0, "c84d599b-2289-47a5-81ed-061fbf16ce50", new DateTime(2026, 3, 9, 19, 55, 49, 876, DateTimeKind.Utc).AddTicks(7775), new DateTime(1990, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user@user.com", true, "", false, false, null, "USER@USER.COM", "User", "AQAAAAIAAYagAAAAEOu4XNo4PNSoxFgFM7DdE+/Fc3QVNdoUJ+LGU0bT0tlSTvDC0PRb9ofBLridqu7fkQ==", null, false, null, "User", "AQAAAAEAACcQAAAAEPG6fy3Z6k0rG+1bF5uV1r+H1l5H3g==nn", null, "Active", false, "User" }
                });
        }
    }
}
