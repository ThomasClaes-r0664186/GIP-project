using Microsoft.EntityFrameworkCore.Migrations;

namespace Gip.Migrations
{
    public partial class courseUser_changeBoolName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Approved",
                table: "CourseUser");

            migrationBuilder.AddColumn<bool>(
                name: "GoedGekeurd",
                table: "CourseUser",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GoedGekeurd",
                table: "CourseUser");

            migrationBuilder.AddColumn<bool>(
                name: "Approved",
                table: "CourseUser",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
