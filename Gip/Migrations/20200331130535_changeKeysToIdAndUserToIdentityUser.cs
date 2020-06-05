using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Gip.Migrations
{
    public partial class changeKeysToIdAndUserToIdentityUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseMoment_User",
                table: "CourseMoment");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseMoment_Course",
                table: "CourseMoment");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseMoment_Schedule",
                table: "CourseMoment");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseMoment_Room",
                table: "CourseMoment");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseUser_User",
                table: "CourseUser");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseUser_Course",
                table: "CourseUser");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Schedule",
                table: "Schedule");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Room",
                table: "Room");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CourseUser",
                table: "CourseUser");

            //migrationBuilder.DropIndex(
            //    name: "PK_CourseUser",
            //    table: "CourseUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CourseMoment",
                table: "CourseMoment");

            //migrationBuilder.DropIndex(
            //    name: "IX_CourseMoment_Userid",
            //    table: "CourseMoment");

            //migrationBuilder.DropIndex(
            //    name: "IX_CourseMoment_Datum_Startmoment_Eindmoment",
            //    table: "CourseMoment");

            //migrationBuilder.DropIndex(
            //    name: "IX_CourseMoment_Gebouw_Verdiep_Nummer",
            //    table: "CourseMoment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Course",
                table: "Course");

            migrationBuilder.DropColumn(
                name: "Userid",
                table: "CourseUser");

            migrationBuilder.DropColumn(
                name: "Vakcode",
                table: "CourseUser");

            migrationBuilder.DropColumn(
                name: "Vakcode",
                table: "CourseMoment");

            migrationBuilder.DropColumn(
                name: "Datum",
                table: "CourseMoment");

            migrationBuilder.DropColumn(
                name: "Gebouw",
                table: "CourseMoment");

            migrationBuilder.DropColumn(
                name: "Verdiep",
                table: "CourseMoment");

            migrationBuilder.DropColumn(
                name: "Nummer",
                table: "CourseMoment");

            migrationBuilder.DropColumn(
                name: "Userid",
                table: "CourseMoment");

            migrationBuilder.DropColumn(
                name: "Startmoment",
                table: "CourseMoment");

            migrationBuilder.DropColumn(
                name: "Eindmoment",
                table: "CourseMoment");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Schedule",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<string>(
                name: "Nummer",
                table: "Room",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Gebouw",
                table: "Room",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Room",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "CourseUser",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "ApplicationUserId",
                table: "CourseUser",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUsersId",
                table: "CourseUser",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CourseId",
                table: "CourseUser",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "CourseMoment",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "ApplicationUserId",
                table: "CourseMoment",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUsersId",
                table: "CourseMoment",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CourseId",
                table: "CourseMoment",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RoomId",
                table: "CourseMoment",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ScheduleId",
                table: "CourseMoment",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Vakcode",
                table: "Course",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Course",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<string>(
                name: "Naam",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VoorNaam",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Schedule",
                table: "Schedule",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Room",
                table: "Room",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CourseUser",
                table: "CourseUser",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CourseMoment",
                table: "CourseMoment",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Course",
                table: "Course",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_CourseUser_ApplicationUsersId",
                table: "CourseUser",
                column: "ApplicationUsersId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseUser_CourseId",
                table: "CourseUser",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseMoment_ApplicationUsersId",
                table: "CourseMoment",
                column: "ApplicationUsersId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseMoment_CourseId",
                table: "CourseMoment",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseMoment_RoomId",
                table: "CourseMoment",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseMoment_ScheduleId",
                table: "CourseMoment",
                column: "ScheduleId");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseMoment_AspNetUsers_ApplicationUsersId",
                table: "CourseMoment",
                column: "ApplicationUsersId",
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
                name: "FK_CourseUser_AspNetUsers_ApplicationUsersId",
                table: "CourseUser",
                column: "ApplicationUsersId",
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseMoment_AspNetUsers_ApplicationUsersId",
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
                name: "FK_CourseUser_AspNetUsers_ApplicationUsersId",
                table: "CourseUser");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseUser_Course_CourseId",
                table: "CourseUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Schedule",
                table: "Schedule");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Room",
                table: "Room");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CourseUser",
                table: "CourseUser");

            migrationBuilder.DropIndex(
                name: "IX_CourseUser_ApplicationUsersId",
                table: "CourseUser");

            migrationBuilder.DropIndex(
                name: "IX_CourseUser_CourseId",
                table: "CourseUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CourseMoment",
                table: "CourseMoment");

            migrationBuilder.DropIndex(
                name: "IX_CourseMoment_ApplicationUsersId",
                table: "CourseMoment");

            migrationBuilder.DropIndex(
                name: "IX_CourseMoment_CourseId",
                table: "CourseMoment");

            migrationBuilder.DropIndex(
                name: "IX_CourseMoment_RoomId",
                table: "CourseMoment");

            migrationBuilder.DropIndex(
                name: "IX_CourseMoment_ScheduleId",
                table: "CourseMoment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Course",
                table: "Course");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Schedule");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Room");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "CourseUser");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "CourseUser");

            migrationBuilder.DropColumn(
                name: "ApplicationUsersId",
                table: "CourseUser");

            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "CourseUser");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "CourseMoment");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "CourseMoment");

            migrationBuilder.DropColumn(
                name: "ApplicationUsersId",
                table: "CourseMoment");

            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "CourseMoment");

            migrationBuilder.DropColumn(
                name: "RoomId",
                table: "CourseMoment");

            migrationBuilder.DropColumn(
                name: "ScheduleId",
                table: "CourseMoment");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Course");

            migrationBuilder.DropColumn(
                name: "Naam",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "VoorNaam",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "Nummer",
                table: "Room",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Gebouw",
                table: "Room",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Userid",
                table: "CourseUser",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Vakcode",
                table: "CourseUser",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Vakcode",
                table: "CourseMoment",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "Datum",
                table: "CourseMoment",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Gebouw",
                table: "CourseMoment",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Verdiep",
                table: "CourseMoment",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Nummer",
                table: "CourseMoment",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Userid",
                table: "CourseMoment",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "Startmoment",
                table: "CourseMoment",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Eindmoment",
                table: "CourseMoment",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<string>(
                name: "Vakcode",
                table: "Course",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Schedule",
                table: "Schedule",
                columns: new[] { "Datum", "Startmoment", "Eindmoment" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Room",
                table: "Room",
                columns: new[] { "Gebouw", "Verdiep", "Nummer" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_CourseUser",
                table: "CourseUser",
                columns: new[] { "Userid", "Vakcode" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_CourseMoment",
                table: "CourseMoment",
                columns: new[] { "Vakcode", "Datum", "Gebouw", "Verdiep", "Nummer", "Userid", "Startmoment", "Eindmoment" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Course",
                table: "Course",
                column: "Vakcode");

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Userid = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Mail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Naam = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VoorNaam = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Userid);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseUser_Vakcode",
                table: "CourseUser",
                column: "Vakcode");

            migrationBuilder.CreateIndex(
                name: "IX_CourseMoment_Userid",
                table: "CourseMoment",
                column: "Userid");

            migrationBuilder.CreateIndex(
                name: "IX_CourseMoment_Datum_Startmoment_Eindmoment",
                table: "CourseMoment",
                columns: new[] { "Datum", "Startmoment", "Eindmoment" });

            migrationBuilder.CreateIndex(
                name: "IX_CourseMoment_Gebouw_Verdiep_Nummer",
                table: "CourseMoment",
                columns: new[] { "Gebouw", "Verdiep", "Nummer" });

            migrationBuilder.AddForeignKey(
                name: "FK_CourseMoment_User_Userid",
                table: "CourseMoment",
                column: "Userid",
                principalTable: "User",
                principalColumn: "Userid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseMoment_Course_Vakcode",
                table: "CourseMoment",
                column: "Vakcode",
                principalTable: "Course",
                principalColumn: "Vakcode",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseMoment_Schedule_Datum_Startmoment_Eindmoment",
                table: "CourseMoment",
                columns: new[] { "Datum", "Startmoment", "Eindmoment" },
                principalTable: "Schedule",
                principalColumns: new[] { "Datum", "Startmoment", "Eindmoment" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseMoment_Room_Gebouw_Verdiep_Nummer",
                table: "CourseMoment",
                columns: new[] { "Gebouw", "Verdiep", "Nummer" },
                principalTable: "Room",
                principalColumns: new[] { "Gebouw", "Verdiep", "Nummer" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseUser_User_Userid",
                table: "CourseUser",
                column: "Userid",
                principalTable: "User",
                principalColumn: "Userid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseUser_Course_Vakcode",
                table: "CourseUser",
                column: "Vakcode",
                principalTable: "Course",
                principalColumn: "Vakcode",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
