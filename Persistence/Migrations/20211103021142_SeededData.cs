using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Migrations
{
    public partial class SeededData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserLanguageProfileUserId",
                table: "UserTerm",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UserLanguageProfile",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    UserId1 = table.Column<string>(type: "TEXT", nullable: false),
                    Language = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLanguageProfile", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_UserLanguageProfile_AspNetUsers_UserId1",
                        column: x => x.UserId1,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserTerm_UserLanguageProfileUserId",
                table: "UserTerm",
                column: "UserLanguageProfileUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLanguageProfile_UserId1",
                table: "UserLanguageProfile",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_UserTerm_UserLanguageProfile_UserLanguageProfileUserId",
                table: "UserTerm",
                column: "UserLanguageProfileUserId",
                principalTable: "UserLanguageProfile",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserTerm_UserLanguageProfile_UserLanguageProfileUserId",
                table: "UserTerm");

            migrationBuilder.DropTable(
                name: "UserLanguageProfile");

            migrationBuilder.DropIndex(
                name: "IX_UserTerm_UserLanguageProfileUserId",
                table: "UserTerm");

            migrationBuilder.DropColumn(
                name: "UserLanguageProfileUserId",
                table: "UserTerm");
        }
    }
}
