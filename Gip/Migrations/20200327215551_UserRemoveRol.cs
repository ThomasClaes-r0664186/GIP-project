using Microsoft.EntityFrameworkCore.Migrations;

namespace Gip.Migrations
{
    public partial class UserRemoveRol : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rol",
                table: "User");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Rol",
                table: "User",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
