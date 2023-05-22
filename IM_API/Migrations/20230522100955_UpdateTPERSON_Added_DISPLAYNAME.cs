using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IM_API.Migrations
{
    public partial class UpdateTPERSON_Added_DISPLAYNAME : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DISPLAYNAME",
                table: "Person",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DISPLAYNAME",
                table: "Person");
        }
    }
}
