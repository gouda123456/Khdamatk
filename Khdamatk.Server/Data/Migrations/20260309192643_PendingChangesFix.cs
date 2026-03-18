using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Khdamatk.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class PendingChangesFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b74ddd14-6340-4840-95c2-db12554843e5",
                column: "CreatedAt",
                value: new DateTime(2026, 3, 9, 19, 26, 39, 957, DateTimeKind.Utc).AddTicks(1623));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b74ddd14-6340-4840-95c2-db12554843eslkna5",
                column: "CreatedAt",
                value: new DateTime(2026, 3, 9, 19, 26, 39, 958, DateTimeKind.Utc).AddTicks(7301));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b74ddd14-6340-4840-95c2-db12554843e5",
                column: "CreatedAt",
                value: new DateTime(2026, 3, 7, 23, 16, 45, 864, DateTimeKind.Utc).AddTicks(4899));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b74ddd14-6340-4840-95c2-db12554843eslkna5",
                column: "CreatedAt",
                value: new DateTime(2026, 3, 7, 23, 16, 45, 865, DateTimeKind.Utc).AddTicks(5414));
        }
    }
}
