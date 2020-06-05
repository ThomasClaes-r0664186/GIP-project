using Microsoft.EntityFrameworkCore.Migrations;

namespace Gip.Migrations
{
    public partial class add_CourseMomentUsersTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CourseMomentUsers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationUserId = table.Column<string>(nullable: true),
                    CoursMomentId = table.Column<int>(nullable: true),
                    CoursemomentsId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseMomentUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseMomentUsers_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CourseMomentUsers_CourseMoment_CoursemomentsId",
                        column: x => x.CoursemomentsId,
                        principalTable: "CourseMoment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseMomentUsers_ApplicationUserId",
                table: "CourseMomentUsers",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseMomentUsers_CoursemomentsId",
                table: "CourseMomentUsers",
                column: "CoursemomentsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CourseMomentUsers");
        }
    }
}
