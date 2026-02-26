using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Khdamatk.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class editePortfolioItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Company",
                table: "PortfolioItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Degree",
                table: "PortfolioItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "PortfolioItems",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FieldOfStudy",
                table: "PortfolioItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SchoolName",
                table: "PortfolioItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "PortfolioItems",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Company",
                table: "PortfolioItems");

            migrationBuilder.DropColumn(
                name: "Degree",
                table: "PortfolioItems");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "PortfolioItems");

            migrationBuilder.DropColumn(
                name: "FieldOfStudy",
                table: "PortfolioItems");

            migrationBuilder.DropColumn(
                name: "SchoolName",
                table: "PortfolioItems");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "PortfolioItems");
        }
    }
}
