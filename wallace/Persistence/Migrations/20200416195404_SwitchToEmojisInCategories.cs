using Microsoft.EntityFrameworkCore.Migrations;

namespace Wallace.Persistence.Migrations
{
    public partial class SwitchToEmojisInCategories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IconUrl",
                table: "Categories");

            migrationBuilder.AddColumn<string>(
                name: "Emoji",
                table: "Categories",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Emoji",
                table: "Categories");

            migrationBuilder.AddColumn<string>(
                name: "IconUrl",
                table: "Categories",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
