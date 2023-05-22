using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IM_API.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Image",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PERSONID = table.Column<int>(type: "int", nullable: false),
                    NAME = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TYPE = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DATA = table.Column<byte[]>(type: "longblob", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Image", x => x.ID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Person",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    USERID = table.Column<int>(type: "int", nullable: false),
                    CREATED = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CHANGED = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Person", x => x.ID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Token",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TOKEN = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    USERID = table.Column<int>(type: "int", nullable: false),
                    CREATED = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CAN_EXPIRE = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    EXPIRES = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DEVICE_NAME = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DEVICE_OS = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DEVICE_APP = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CHANGED = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Token", x => x.ID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PASSWORD = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CREATED = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CHANGED = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    USERNAME = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EMAIL = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FIRSTNAME = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LASTNAME = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ROLE = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    VERIFIED = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ACTIVE = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    BIRTHDATE = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.ID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UserOptions",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    USERID = table.Column<int>(type: "int", nullable: false),
                    SHOW_EMAIL = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    SHOW_BIRTHDATE = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    SHOW_FIRSTNAME = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    SHOW_LASTNAME = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    SHOW_ROLE = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    SHOW_CREATED = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    SHOW_CHANGED = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CREATED = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CHANGED = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserOptions", x => x.ID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Image_PERSONID",
                table: "Image",
                column: "PERSONID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Person_USERID",
                table: "Person",
                column: "USERID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Token_TOKEN",
                table: "Token",
                column: "TOKEN",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_EMAIL",
                table: "User",
                column: "EMAIL",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_USERNAME",
                table: "User",
                column: "USERNAME",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserOptions_USERID",
                table: "UserOptions",
                column: "USERID",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Image");

            migrationBuilder.DropTable(
                name: "Person");

            migrationBuilder.DropTable(
                name: "Token");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "UserOptions");
        }
    }
}
