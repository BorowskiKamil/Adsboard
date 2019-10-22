using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Adsboard.Services.Categories.Migrations
{
    public partial class CategoryEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "categories",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    name = table.Column<string>(type: "VARCHAR(255)", nullable: false),
                    created_at = table.Column<DateTimeOffset>(nullable: false),
                    creator_id = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_categories", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "categories");
        }
    }
}
