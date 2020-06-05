using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Gip.Migrations
{
    public partial class DbInit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            /*
            migrationBuilder.CreateTable(
                name: "Course",
                columns: table => new
                {
                    Vakcode = table.Column<string>(nullable: false),
                    Titel = table.Column<string>(nullable: true),
                    Studiepunten = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Course", x => x.Vakcode);
                });

            migrationBuilder.CreateTable(
                name: "Room",
                columns: table => new
                {
                    Gebouw = table.Column<string>(nullable: false),
                    Verdiep = table.Column<int>(nullable: false),
                    Nummer = table.Column<string>(nullable: false),
                    Middelen = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    Capaciteit = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Room", x => new { x.Gebouw, x.Verdiep, x.Nummer });
                });

            migrationBuilder.CreateTable(
                name: "Schedule",
                columns: table => new
                {
                    Datum = table.Column<DateTime>(nullable: false),
                    Startmoment = table.Column<DateTime>(nullable: false),
                    Eindmoment = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schedule", x => new { x.Datum, x.Startmoment, x.Eindmoment });
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Userid = table.Column<string>(nullable: false),
                    Naam = table.Column<string>(nullable: true),
                    Mail = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Userid);
                });

            migrationBuilder.CreateTable(
                name: "CourseMoment",
                columns: table => new
                {
                    Eindmoment = table.Column<DateTime>(nullable: false),
                    Userid = table.Column<string>(nullable: false),
                    Vakcode = table.Column<string>(nullable: false),
                    Verdiep = table.Column<int>(nullable: false),
                    Nummer = table.Column<string>(nullable: false),
                    Gebouw = table.Column<string>(nullable: false),
                    Datum = table.Column<DateTime>(nullable: false),
                    Startmoment = table.Column<DateTime>(nullable: false),
                    LessenLijst = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseMoment", x => new { x.Vakcode, x.Datum, x.Gebouw, x.Verdiep, x.Nummer, x.Userid, x.Startmoment, x.Eindmoment });
                    table.ForeignKey(
                        name: "FK_CourseMoment_User_Userid",
                        column: x => x.Userid,
                        principalTable: "User",
                        principalColumn: "Userid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseMoment_Course_Vakcode",
                        column: x => x.Vakcode,
                        principalTable: "Course",
                        principalColumn: "Vakcode",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseMoment_Schedule_Datum_Startmoment_Eindmoment",
                        columns: x => new { x.Datum, x.Startmoment, x.Eindmoment },
                        principalTable: "Schedule",
                        principalColumns: new[] { "Datum", "Startmoment", "Eindmoment" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseMoment_Room_Gebouw_Verdiep_Nummer",
                        columns: x => new { x.Gebouw, x.Verdiep, x.Nummer },
                        principalTable: "Room",
                        principalColumns: new[] { "Gebouw", "Verdiep", "Nummer" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CourseUser",
                columns: table => new
                {
                    Userid = table.Column<string>(nullable: false),
                    Vakcode = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseUser", x => new { x.Userid, x.Vakcode });
                    table.ForeignKey(
                        name: "FK_CourseUser_User_Userid",
                        column: x => x.Userid,
                        principalTable: "User",
                        principalColumn: "Userid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseUser_Course_Vakcode",
                        column: x => x.Vakcode,
                        principalTable: "Course",
                        principalColumn: "Vakcode",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_CourseUser_Vakcode",
                table: "CourseUser",
                column: "Vakcode");
                */
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CourseMoment");

            migrationBuilder.DropTable(
                name: "CourseUser");

            migrationBuilder.DropTable(
                name: "Schedule");

            migrationBuilder.DropTable(
                name: "Room");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Course");
        }
    }
}
