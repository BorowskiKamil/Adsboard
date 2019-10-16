using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Adsboard.Services.Adverts.Migrations
{
    public partial class ArchivedAtField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "archived_at",
                table: "adverts",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "archived_at",
                table: "adverts");
        }
    }
}
