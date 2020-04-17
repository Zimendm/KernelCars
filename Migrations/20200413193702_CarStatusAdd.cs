using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KernelCars.Migrations
{
    public partial class CarStatusAdd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UnitId",
                table: "Cars",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Statuses",
                columns: table => new
                {
                    StatusID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    State = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Statuses", x => x.StatusID);
                });

            migrationBuilder.CreateTable(
                name: "CarStatuses",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BeginDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    CarId = table.Column<int>(nullable: false),
                    CarId1 = table.Column<long>(nullable: true),
                    StatusId = table.Column<int>(nullable: false),
                    UnitId = table.Column<int>(nullable: false),
                    Comment = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarStatuses", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CarStatuses_Cars_CarId1",
                        column: x => x.CarId1,
                        principalTable: "Cars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CarStatuses_Statuses_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Statuses",
                        principalColumn: "StatusID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CarStatuses_Units_UnitId",
                        column: x => x.UnitId,
                        principalTable: "Units",
                        principalColumn: "UnitId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cars_UnitId",
                table: "Cars",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_CarStatuses_CarId1",
                table: "CarStatuses",
                column: "CarId1");

            migrationBuilder.CreateIndex(
                name: "IX_CarStatuses_StatusId",
                table: "CarStatuses",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_CarStatuses_UnitId",
                table: "CarStatuses",
                column: "UnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cars_Units_UnitId",
                table: "Cars",
                column: "UnitId",
                principalTable: "Units",
                principalColumn: "UnitId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cars_Units_UnitId",
                table: "Cars");

            migrationBuilder.DropTable(
                name: "CarStatuses");

            migrationBuilder.DropTable(
                name: "Statuses");

            migrationBuilder.DropIndex(
                name: "IX_Cars_UnitId",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "UnitId",
                table: "Cars");
        }
    }
}
