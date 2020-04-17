using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KernelCars.Migrations
{
    public partial class AddCarUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CarUser",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartUsingDate = table.Column<DateTime>(nullable: false),
                    EndUsingDate = table.Column<DateTime>(nullable: true),
                    Comment = table.Column<string>(nullable: true),
                    EmployeeId = table.Column<int>(nullable: false),
                    CarId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarUser", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CarUser_Cars_CarId",
                        column: x => x.CarId,
                        principalTable: "Cars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CarUser_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CarUser_CarId",
                table: "CarUser",
                column: "CarId");

            migrationBuilder.CreateIndex(
                name: "IX_CarUser_EmployeeId",
                table: "CarUser",
                column: "EmployeeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CarUser");
        }
    }
}
