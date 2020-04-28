using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Gip.Migrations
{
    public partial class AddFieldOfStudyAndBirthdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FieldOfStudyId",
                table: "Course",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "GeboorteDatum",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "FieldOfStudy",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RichtingCode = table.Column<string>(nullable: true),
                    RichtingTitel = table.Column<string>(nullable: true),
                    RichtingStudiepunten = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FieldOfStudy", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Course_FieldOfStudyId",
                table: "Course",
                column: "FieldOfStudyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Course_FieldOfStudy_FieldOfStudyId",
                table: "Course",
                column: "FieldOfStudyId",
                principalTable: "FieldOfStudy",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Course_FieldOfStudy_FieldOfStudyId",
                table: "Course");

            migrationBuilder.DropTable(
                name: "FieldOfStudy");

            migrationBuilder.DropIndex(
                name: "IX_Course_FieldOfStudyId",
                table: "Course");

            migrationBuilder.DropColumn(
                name: "FieldOfStudyId",
                table: "Course");

            migrationBuilder.DropColumn(
                name: "GeboorteDatum",
                table: "AspNetUsers");
        }
    }
}
