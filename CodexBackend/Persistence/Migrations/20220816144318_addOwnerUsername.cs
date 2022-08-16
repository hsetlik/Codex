using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    public partial class addOwnerUsername : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NormalizedTermValue",
                table: "UserTerms",
                newName: "TermValue");

            migrationBuilder.AddColumn<string>(
                name: "OwnerUsername",
                table: "UserTerms",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OwnerUsername",
                table: "UserTerms");

            migrationBuilder.RenameColumn(
                name: "TermValue",
                table: "UserTerms",
                newName: "NormalizedTermValue");
        }
    }
}
