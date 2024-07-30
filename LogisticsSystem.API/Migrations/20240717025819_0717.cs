using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LogisticsSystem.API.Migrations
{
    public partial class _0717 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AirTransportDetatail");

            migrationBuilder.RenameColumn(
                name: "MemberId",
                table: "Customer",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "MemberId",
                table: "AirTransport",
                newName: "UserId");

            migrationBuilder.CreateTable(
                name: "AirTransportDetail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AirTransportId = table.Column<int>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    ReceiveDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Count = table.Column<int>(type: "INTEGER", nullable: false),
                    Weight = table.Column<double>(type: "REAL", nullable: false),
                    Volume = table.Column<double>(type: "REAL", nullable: false),
                    Height = table.Column<double>(type: "REAL", nullable: false),
                    Width = table.Column<double>(type: "REAL", nullable: false),
                    Length = table.Column<double>(type: "REAL", nullable: false),
                    Note = table.Column<string>(type: "TEXT", nullable: false),
                    Tag = table.Column<string>(type: "TEXT", nullable: true),
                    InsertDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AirTransportDetail", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AirTransportDetail");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Customer",
                newName: "MemberId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "AirTransport",
                newName: "MemberId");

            migrationBuilder.CreateTable(
                name: "AirTransportDetatail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AirTransportId = table.Column<int>(type: "INTEGER", nullable: false),
                    Count = table.Column<int>(type: "INTEGER", nullable: false),
                    Height = table.Column<double>(type: "REAL", nullable: false),
                    InsertDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Length = table.Column<double>(type: "REAL", nullable: false),
                    MemberId = table.Column<int>(type: "INTEGER", nullable: false),
                    Note = table.Column<string>(type: "TEXT", nullable: false),
                    ReceiveDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Tag = table.Column<string>(type: "TEXT", nullable: true),
                    Volume = table.Column<double>(type: "REAL", nullable: false),
                    Weight = table.Column<double>(type: "REAL", nullable: false),
                    Width = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AirTransportDetatail", x => x.Id);
                });
        }
    }
}
