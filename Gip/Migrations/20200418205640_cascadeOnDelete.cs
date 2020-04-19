using Microsoft.EntityFrameworkCore.Migrations;

namespace Gip.Migrations
{
    public partial class cascadeOnDelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseMoment_AspNetUsers_ApplicationUserId",
                table: "CourseMoment");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseMoment_Course_CourseId",
                table: "CourseMoment");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseMoment_Room_RoomId",
                table: "CourseMoment");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseMoment_Schedule_ScheduleId",
                table: "CourseMoment");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseMomentUsers_AspNetUsers_ApplicationUserId",
                table: "CourseMomentUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseMomentUsers_CourseMoment_CoursemomentsId",
                table: "CourseMomentUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseUser_AspNetUsers_ApplicationUserId",
                table: "CourseUser");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseUser_Course_CourseId",
                table: "CourseUser");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseMoment_AspNetUsers_ApplicationUserId",
                table: "CourseMoment",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseMoment_Course_CourseId",
                table: "CourseMoment",
                column: "CourseId",
                principalTable: "Course",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseMoment_Room_RoomId",
                table: "CourseMoment",
                column: "RoomId",
                principalTable: "Room",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseMoment_Schedule_ScheduleId",
                table: "CourseMoment",
                column: "ScheduleId",
                principalTable: "Schedule",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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

            migrationBuilder.AddForeignKey(
                name: "FK_CourseUser_AspNetUsers_ApplicationUserId",
                table: "CourseUser",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseUser_Course_CourseId",
                table: "CourseUser",
                column: "CourseId",
                principalTable: "Course",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseMoment_AspNetUsers_ApplicationUserId",
                table: "CourseMoment");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseMoment_Course_CourseId",
                table: "CourseMoment");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseMoment_Room_RoomId",
                table: "CourseMoment");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseMoment_Schedule_ScheduleId",
                table: "CourseMoment");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseMomentUsers_AspNetUsers_ApplicationUserId",
                table: "CourseMomentUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseMomentUsers_CourseMoment_CoursemomentsId",
                table: "CourseMomentUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseUser_AspNetUsers_ApplicationUserId",
                table: "CourseUser");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseUser_Course_CourseId",
                table: "CourseUser");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseMoment_AspNetUsers_ApplicationUserId",
                table: "CourseMoment",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseMoment_Course_CourseId",
                table: "CourseMoment",
                column: "CourseId",
                principalTable: "Course",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseMoment_Room_RoomId",
                table: "CourseMoment",
                column: "RoomId",
                principalTable: "Room",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseMoment_Schedule_ScheduleId",
                table: "CourseMoment",
                column: "ScheduleId",
                principalTable: "Schedule",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

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
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseUser_AspNetUsers_ApplicationUserId",
                table: "CourseUser",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseUser_Course_CourseId",
                table: "CourseUser",
                column: "CourseId",
                principalTable: "Course",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
