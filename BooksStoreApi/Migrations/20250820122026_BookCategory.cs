using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BooksStoreApi.Migrations
{
    public partial class BookCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ParentId",
                table: "BookCategories",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BookCategories_ParentId",
                table: "BookCategories",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookCategories_BookCategories_ParentId",
                table: "BookCategories",
                column: "ParentId",
                principalTable: "BookCategories",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookCategories_BookCategories_ParentId",
                table: "BookCategories");

            migrationBuilder.DropIndex(
                name: "IX_BookCategories_ParentId",
                table: "BookCategories");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "BookCategories");
        }
    }
}
