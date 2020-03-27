using Microsoft.EntityFrameworkCore.Migrations;

namespace Gip.Migrations
{
    public partial class CMLessenLijstUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LessenLijst",
                table: "CourseMoment");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LessenLijst",
                table: "CourseMoment",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
