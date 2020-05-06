using Microsoft.EntityFrameworkCore.Migrations;

namespace Gip.Migrations
{
    public partial class addTypeToFieldOfStudy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "FieldOfStudy",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "FieldOfStudy");
        }
    }
}
