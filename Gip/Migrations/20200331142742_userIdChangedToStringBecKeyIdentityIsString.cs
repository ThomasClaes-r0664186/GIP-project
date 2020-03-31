using Microsoft.EntityFrameworkCore.Migrations;

namespace Gip.Migrations
{
    public partial class userIdChangedToStringBecKeyIdentityIsString : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseMoment_AspNetUsers_ApplicationUsersId",
                table: "CourseMoment");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseUser_AspNetUsers_ApplicationUsersId",
                table: "CourseUser");

            migrationBuilder.DropIndex(
                name: "IX_CourseUser_ApplicationUsersId",
                table: "CourseUser");

            migrationBuilder.DropIndex(
                name: "IX_CourseMoment_ApplicationUsersId",
                table: "CourseMoment");

            migrationBuilder.DropColumn(
                name: "ApplicationUsersId",
                table: "CourseUser");

            migrationBuilder.DropColumn(
                name: "ApplicationUsersId",
                table: "CourseMoment");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "CourseUser",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "CourseMoment",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CourseUser_ApplicationUserId",
                table: "CourseUser",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseMoment_ApplicationUserId",
                table: "CourseMoment",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseMoment_AspNetUsers_ApplicationUserId",
                table: "CourseMoment",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseUser_AspNetUsers_ApplicationUserId",
                table: "CourseUser",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseMoment_AspNetUsers_ApplicationUserId",
                table: "CourseMoment");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseUser_AspNetUsers_ApplicationUserId",
                table: "CourseUser");

            migrationBuilder.DropIndex(
                name: "IX_CourseUser_ApplicationUserId",
                table: "CourseUser");

            migrationBuilder.DropIndex(
                name: "IX_CourseMoment_ApplicationUserId",
                table: "CourseMoment");

            migrationBuilder.AlterColumn<int>(
                name: "ApplicationUserId",
                table: "CourseUser",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUsersId",
                table: "CourseUser",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ApplicationUserId",
                table: "CourseMoment",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUsersId",
                table: "CourseMoment",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CourseUser_ApplicationUsersId",
                table: "CourseUser",
                column: "ApplicationUsersId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseMoment_ApplicationUsersId",
                table: "CourseMoment",
                column: "ApplicationUsersId");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseMoment_AspNetUsers_ApplicationUsersId",
                table: "CourseMoment",
                column: "ApplicationUsersId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseUser_AspNetUsers_ApplicationUsersId",
                table: "CourseUser",
                column: "ApplicationUsersId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
