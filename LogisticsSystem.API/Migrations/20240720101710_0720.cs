using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LogisticsSystem.API.Migrations
{
    public partial class _0720 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExpressTransport",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: true),
                    CustomerId = table.Column<int>(type: "INTEGER", nullable: false),
                    OrderNumber = table.Column<string>(type: "TEXT", nullable: false),
                    Channel = table.Column<string>(type: "TEXT", nullable: false),
                    Count = table.Column<int>(type: "INTEGER", nullable: false),
                    Volume = table.Column<double>(type: "REAL", nullable: false),
                    Weight = table.Column<double>(type: "REAL", nullable: false),
                    Price = table.Column<double>(type: "REAL", nullable: false),
                    SourcePlace = table.Column<string>(type: "TEXT", nullable: false),
                    TargetPlace = table.Column<string>(type: "TEXT", nullable: false),
                    SendData = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Note = table.Column<string>(type: "TEXT", nullable: false),
                    CalcWeight = table.Column<double>(type: "REAL", nullable: false),
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
                    table.PrimaryKey("PK_ExpressTransport", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExpressTransportDetail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    ExpressTransportId = table.Column<int>(type: "INTEGER", nullable: false),
                    Productor = table.Column<string>(type: "TEXT", nullable: false),
                    ReceiveDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Note = table.Column<string>(type: "TEXT", nullable: false),
                    Tag = table.Column<string>(type: "TEXT", nullable: true),
                    InsertDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpressTransportDetail", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SeaTransport",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: true),
                    CustomerId = table.Column<int>(type: "INTEGER", nullable: false),
                    BoxModel = table.Column<string>(type: "TEXT", nullable: false),
                    BoxNumber = table.Column<string>(type: "TEXT", nullable: false),
                    SourcePlace = table.Column<string>(type: "TEXT", nullable: false),
                    TargetPlace = table.Column<string>(type: "TEXT", nullable: false),
                    SendData = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Batch = table.Column<string>(type: "TEXT", nullable: false),
                    Count = table.Column<int>(type: "INTEGER", nullable: false),
                    Volume = table.Column<double>(type: "REAL", nullable: false),
                    Weight = table.Column<double>(type: "REAL", nullable: false),
                    Note = table.Column<string>(type: "TEXT", nullable: false),
                    Price = table.Column<double>(type: "REAL", nullable: false),
                    OtherMoney = table.Column<double>(type: "REAL", nullable: false),
                    OtherDescription = table.Column<string>(type: "TEXT", nullable: false),
                    PayMoney = table.Column<double>(type: "REAL", nullable: false),
                    PayDescription = table.Column<double>(type: "REAL", nullable: false),
                    PayDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Tag = table.Column<string>(type: "TEXT", nullable: true),
                    InsertDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeaTransport", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SeaTransportDetail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SeaTransportId = table.Column<int>(type: "INTEGER", nullable: false),
                    USerId = table.Column<int>(type: "INTEGER", nullable: false),
                    Productor = table.Column<string>(type: "TEXT", nullable: false),
                    ReceiveDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Count = table.Column<int>(type: "INTEGER", nullable: true),
                    Volume = table.Column<double>(type: "REAL", nullable: true),
                    Note = table.Column<string>(type: "TEXT", nullable: false),
                    Tag = table.Column<string>(type: "TEXT", nullable: true),
                    InsertDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeaTransportDetail", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExpressTransport");

            migrationBuilder.DropTable(
                name: "ExpressTransportDetail");

            migrationBuilder.DropTable(
                name: "SeaTransport");

            migrationBuilder.DropTable(
                name: "SeaTransportDetail");
        }
    }
}
