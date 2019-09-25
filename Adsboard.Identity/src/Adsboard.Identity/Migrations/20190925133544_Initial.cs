using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Adsboard.Services.Identity.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "identities",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    email = table.Column<string>(type: "VARCHAR(255)", nullable: false),
                    role = table.Column<string>(type: "varchar(255)", nullable: true, defaultValue: "user"),
                    password_hash = table.Column<string>(type: "varchar(255)", nullable: true),
                    created_at = table.Column<DateTime>(nullable: false),
                    updated_at = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_identities", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "refresh_tokens",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    identity_id = table.Column<Guid>(nullable: false),
                    token = table.Column<string>(type: "varchar(255)", nullable: false),
                    created_at = table.Column<DateTime>(nullable: false),
                    revoked_at = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_refresh_tokens", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "identities");

            migrationBuilder.DropTable(
                name: "refresh_tokens");
        }
    }
}
