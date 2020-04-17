using Microsoft.EntityFrameworkCore.Migrations;

namespace KernelCars.Migrations
{
    public partial class CarStatusAdd1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cars_Units_UnitId",
                table: "Cars");

            migrationBuilder.DropIndex(
                name: "IX_Cars_UnitId",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "UnitId",
                table: "Cars");

            migrationBuilder.AddColumn<int>(
                name: "EmployeeId",
                table: "CarStatuses",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CarStatuses_EmployeeId",
                table: "CarStatuses",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_CarStatuses_Employees_EmployeeId",
                table: "CarStatuses",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarStatuses_Employees_EmployeeId",
                table: "CarStatuses");

            migrationBuilder.DropIndex(
                name: "IX_CarStatuses_EmployeeId",
                table: "CarStatuses");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "CarStatuses");

            migrationBuilder.AddColumn<int>(
                name: "UnitId",
                table: "Cars",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cars_UnitId",
                table: "Cars",
                column: "UnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cars_Units_UnitId",
                table: "Cars",
                column: "UnitId",
                principalTable: "Units",
                principalColumn: "UnitId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
