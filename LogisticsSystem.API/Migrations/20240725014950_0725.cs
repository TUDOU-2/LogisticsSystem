using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LogisticsSystem.API.Migrations
{
    public partial class _0725 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Account",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Account",
                table: "Users");
        }
    }
}
