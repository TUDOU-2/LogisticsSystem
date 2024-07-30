using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LogisticsSystem.API.Migrations
{
    public partial class Logistics : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AirTransport",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MemberId = table.Column<int>(type: "INTEGER", nullable: true),
                    CustomerId = table.Column<int>(type: "INTEGER", nullable: false),
                    OrderNumber = table.Column<string>(type: "TEXT", nullable: false),
                    SourcePlace = table.Column<string>(type: "TEXT", nullable: false),
                    TargetPlace = table.Column<string>(type: "TEXT", nullable: false),
                    SendData = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Batch = table.Column<string>(type: "TEXT", nullable: false),
                    Note = table.Column<string>(type: "TEXT", nullable: false),
                    CalcWeight = table.Column<double>(type: "REAL", nullable: false),
                    Price = table.Column<double>(type: "REAL", nullable: false),
                    OtherMoney = table.Column<double>(type: "REAL", nullable: false),
                    OtherDescription = table.Column<string>(type: "TEXT", nullable: false),
                    PayMoney = table.Column<double>(type: "REAL", nullable: false),
                    PayDescription = table.Column<string>(type: "TEXT", nullable: false),
                    PayDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Tag = table.Column<string>(type: "TEXT", nullable: true),
                    InsertDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AirTransport", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AirTransportDetatail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AirTransportId = table.Column<int>(type: "INTEGER", nullable: false),
                    MemberId = table.Column<int>(type: "INTEGER", nullable: false),
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
                    table.PrimaryKey("PK_AirTransportDetatail", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MemberId = table.Column<int>(type: "INTEGER", nullable: true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Telephone = table.Column<string>(type: "TEXT", nullable: false),
                    Nation = table.Column<string>(type: "TEXT", nullable: false),
                    Address = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Tag = table.Column<string>(type: "TEXT", nullable: true),
                    InsertDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Password = table.Column<string>(type: "TEXT", nullable: false),
                    Level = table.Column<int>(type: "INTEGER", nullable: false),
                    Tag = table.Column<string>(type: "TEXT", nullable: true),
                    InsertDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AirTransport");

            migrationBuilder.DropTable(
                name: "AirTransportDetatail");

            migrationBuilder.DropTable(
                name: "Customer");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
