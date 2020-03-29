using Microsoft.EntityFrameworkCore.Migrations;

namespace Gip.Migrations
{
    public partial class CMLessenLijstUpdateAddedAgain : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LessenLijst",
                table: "CourseMoment",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LessenLijst",
                table: "CourseMoment");
        }
    }
}
