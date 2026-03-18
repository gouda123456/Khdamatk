using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Khdamatk.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class EditServiceAndJobsEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Services",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Services",
                newName: "ShortDescription");

            migrationBuilder.RenameColumn(
                name: "CoverLetter",
                table: "jobOffers",
                newName: "ExperienceLevel");

            migrationBuilder.AddColumn<string>(
                name: "Concepts",
                table: "Services",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "DeliverTimeInDays",
                table: "Services",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<string>(
                name: "DetailedDescription",
                table: "Services",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "RevisionCount",
                table: "Services",
                type: "int",
                nullable: false,
                defaultValue: 0);

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

            migrationBuilder.AddColumn<int>(
                name: "JobOfferId",
                table: "Medias",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Deadline",
                table: "jobOffers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "jobOffers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SimilarWorkExamplesURL",
                table: "jobOffers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TimeCommitment",
                table: "jobOffers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Medias_JobOfferId",
                table: "Medias",
                column: "JobOfferId");

            migrationBuilder.AddForeignKey(
                name: "FK_Medias_jobOffers_JobOfferId",
                table: "Medias",
                column: "JobOfferId",
                principalTable: "jobOffers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Medias_jobOffers_JobOfferId",
                table: "Medias");

            migrationBuilder.DropIndex(
                name: "IX_Medias_JobOfferId",
                table: "Medias");

            migrationBuilder.DropColumn(
                name: "Concepts",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "DeliverTimeInDays",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "DetailedDescription",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "RevisionCount",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "invoiceId",
                table: "serviceOrders");

            migrationBuilder.DropColumn(
                name: "invoiceKey",
                table: "serviceOrders");

            migrationBuilder.DropColumn(
                name: "JobOfferId",
                table: "Medias");

            migrationBuilder.DropColumn(
                name: "Deadline",
                table: "jobOffers");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "jobOffers");

            migrationBuilder.DropColumn(
                name: "SimilarWorkExamplesURL",
                table: "jobOffers");

            migrationBuilder.DropColumn(
                name: "TimeCommitment",
                table: "jobOffers");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Services",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "ShortDescription",
                table: "Services",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "ExperienceLevel",
                table: "jobOffers",
                newName: "CoverLetter");
        }
    }
}
