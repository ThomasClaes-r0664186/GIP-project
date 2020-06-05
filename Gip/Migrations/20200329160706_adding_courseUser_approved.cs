using Microsoft.EntityFrameworkCore.Migrations;

namespace Gip.Migrations
{
    public partial class adding_courseUser_approved : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Approved",
                table: "CourseUser",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Approved",
                table: "CourseUser");
        }
    }
}
