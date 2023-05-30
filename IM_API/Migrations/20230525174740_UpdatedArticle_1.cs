using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IM_API.Migrations
{
    public partial class UpdatedArticle_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "QUANTITY",
                table: "Article",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QUANTITY",
                table: "Article");
        }
    }
}
