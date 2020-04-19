using Microsoft.EntityFrameworkCore.Migrations;

namespace Gip.Migrations
{
    public partial class cascadeSetNullCMU : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseMomentUsers_AspNetUsers_ApplicationUserId",
                table: "CourseMomentUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseMomentUsers_CourseMoment_CoursemomentsId",
                table: "CourseMomentUsers");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseMomentUsers_AspNetUsers_ApplicationUserId",
                table: "CourseMomentUsers",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseMomentUsers_CourseMoment_CoursemomentsId",
                table: "CourseMomentUsers",
                column: "CoursemomentsId",
                principalTable: "CourseMoment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseMomentUsers_AspNetUsers_ApplicationUserId",
                table: "CourseMomentUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseMomentUsers_CourseMoment_CoursemomentsId",
                table: "CourseMomentUsers");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseMomentUsers_AspNetUsers_ApplicationUserId",
                table: "CourseMomentUsers",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseMomentUsers_CourseMoment_CoursemomentsId",
                table: "CourseMomentUsers",
                column: "CoursemomentsId",
                principalTable: "CourseMoment",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }
    }
}
