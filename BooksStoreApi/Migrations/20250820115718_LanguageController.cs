using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BooksStoreApi.Migrations
{
    public partial class LanguageController : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Languages",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                table: "Languages");
        }
    }
}
