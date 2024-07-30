using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LogisticsSystem.API.Migrations
{
    public partial class _07231 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Count",
                table: "SeaTransport");

            migrationBuilder.DropColumn(
                name: "Volume",
                table: "SeaTransport");

            migrationBuilder.DropColumn(
                name: "Weight",
                table: "SeaTransport");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Count",
                table: "SeaTransport",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "Volume",
                table: "SeaTransport",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Weight",
                table: "SeaTransport",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
