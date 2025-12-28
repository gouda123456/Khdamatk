using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Khdamatk.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class updatepasswords : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentMethod",
                table: "PaymentTransactions");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b74ddd14-6340-4840-95c2-db12554843e5",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEDTQ2MJwg8G7sNyX4vUuSzK8tnhxhzDsZpWvc8jt01mVjPURiVuM4G80qBh84/zwgA==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b74ddd14-6340-4840-95c2-db12554843eslkna5",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEOu4XNo4PNSoxFgFM7DdE+/Fc3QVNdoUJ+LGU0bT0tlSTvDC0PRb9ofBLridqu7fkQ==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PaymentMethod",
                table: "PaymentTransactions",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b74ddd14-6340-4840-95c2-db12554843e5",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEMt3R8nCBG3h9GryaG11Qj68nl5EDP+ZKBjCV4HOD2lx+LN85tvOvgMeKNOnMehynA==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b74ddd14-6340-4840-95c2-db12554843eslkna5",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAED3PZWFMrhk5LqhFU9+aNGcFCtf09zTJbUOQrteefrAW9rYdS9r+4gbzaOPVc8XM5Q==");
        }
    }
}
