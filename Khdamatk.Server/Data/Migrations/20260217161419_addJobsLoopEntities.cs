using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Khdamatk.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class addJobsLoopEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ProviderSkills",
                table: "ProviderSkills");

            migrationBuilder.DropIndex(
                name: "IX_ProviderSkills_ServiceProviderProfileId",
                table: "ProviderSkills");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ProviderSkills");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "ProviderSkills");

            migrationBuilder.RenameColumn(
                name: "ExperienceLevel",
                table: "ProviderSkills",
                newName: "SkillId");

            migrationBuilder.AddColumn<int>(
                name: "MyLevel",
                table: "ProviderSkills",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "JobPostId",
                table: "Medias",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ExperienceLevel",
                table: "JobPosts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ProjectLength",
                table: "JobPosts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TimeCommitment",
                table: "JobPosts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsAccepted",
                table: "jobOffers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "NetAmount",
                table: "jobOffers",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProviderSkills",
                table: "ProviderSkills",
                columns: new[] { "ServiceProviderProfileId", "SkillId" });

            migrationBuilder.CreateTable(
                name: "Skills",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skills", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JobSkillRequirements",
                columns: table => new
                {
                    JobPostId = table.Column<int>(type: "int", nullable: false),
                    SkillId = table.Column<int>(type: "int", nullable: false),
                    RequiredLevel = table.Column<int>(type: "int", nullable: false, defaultValue: 1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobSkillRequirements", x => new { x.JobPostId, x.SkillId });
                    table.ForeignKey(
                        name: "FK_JobSkillRequirements_JobPosts_JobPostId",
                        column: x => x.JobPostId,
                        principalTable: "JobPosts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_JobSkillRequirements_Skills_SkillId",
                        column: x => x.SkillId,
                        principalTable: "Skills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProviderSkills_SkillId",
                table: "ProviderSkills",
                column: "SkillId");

            migrationBuilder.CreateIndex(
                name: "IX_Medias_JobPostId",
                table: "Medias",
                column: "JobPostId");

            migrationBuilder.CreateIndex(
                name: "IX_JobSkillRequirements_SkillId",
                table: "JobSkillRequirements",
                column: "SkillId");

            migrationBuilder.AddForeignKey(
                name: "FK_Medias_JobPosts_JobPostId",
                table: "Medias",
                column: "JobPostId",
                principalTable: "JobPosts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProviderSkills_Skills_SkillId",
                table: "ProviderSkills",
                column: "SkillId",
                principalTable: "Skills",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Medias_JobPosts_JobPostId",
                table: "Medias");

            migrationBuilder.DropForeignKey(
                name: "FK_ProviderSkills_Skills_SkillId",
                table: "ProviderSkills");

            migrationBuilder.DropTable(
                name: "JobSkillRequirements");

            migrationBuilder.DropTable(
                name: "Skills");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProviderSkills",
                table: "ProviderSkills");

            migrationBuilder.DropIndex(
                name: "IX_ProviderSkills_SkillId",
                table: "ProviderSkills");

            migrationBuilder.DropIndex(
                name: "IX_Medias_JobPostId",
                table: "Medias");

            migrationBuilder.DropColumn(
                name: "MyLevel",
                table: "ProviderSkills");

            migrationBuilder.DropColumn(
                name: "JobPostId",
                table: "Medias");

            migrationBuilder.DropColumn(
                name: "ExperienceLevel",
                table: "JobPosts");

            migrationBuilder.DropColumn(
                name: "ProjectLength",
                table: "JobPosts");

            migrationBuilder.DropColumn(
                name: "TimeCommitment",
                table: "JobPosts");

            migrationBuilder.DropColumn(
                name: "IsAccepted",
                table: "jobOffers");

            migrationBuilder.DropColumn(
                name: "NetAmount",
                table: "jobOffers");

            migrationBuilder.RenameColumn(
                name: "SkillId",
                table: "ProviderSkills",
                newName: "ExperienceLevel");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "ProviderSkills",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "ProviderSkills",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProviderSkills",
                table: "ProviderSkills",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ProviderSkills_ServiceProviderProfileId",
                table: "ProviderSkills",
                column: "ServiceProviderProfileId");
        }
    }
}
