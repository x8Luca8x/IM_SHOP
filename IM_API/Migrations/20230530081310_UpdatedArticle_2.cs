using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IM_API.Migrations
{
    public partial class UpdatedArticle_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "INDEX",
                table: "Image",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "TAGS",
                table: "Article",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "INDEX",
                table: "Image");

            migrationBuilder.DropColumn(
                name: "TAGS",
                table: "Article");
        }
    }
}
