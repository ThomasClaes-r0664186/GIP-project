using Microsoft.EntityFrameworkCore.Migrations;

namespace Gip.Migrations
{
    public partial class BoodschapAfwijzing : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "GoedGekeurd",
                table: "CourseUser",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddColumn<string>(
                name: "AfwijzingBeschr",
                table: "CourseUser",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AfwijzingBeschr",
                table: "CourseUser");

            migrationBuilder.AlterColumn<bool>(
                name: "GoedGekeurd",
                table: "CourseUser",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);
        }
    }
}
