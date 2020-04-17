using Microsoft.EntityFrameworkCore.Migrations;

namespace KernelCars.Migrations
{
    public partial class CarStatusAdd3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarStatuses_Cars_CarId",
                table: "CarStatuses");

            migrationBuilder.DropForeignKey(
                name: "FK_CarStatuses_Employees_EmployeeId",
                table: "CarStatuses");

            migrationBuilder.DropForeignKey(
                name: "FK_CarStatuses_Units_UnitId",
                table: "CarStatuses");

            migrationBuilder.DropIndex(
                name: "IX_CarStatuses_CarId",
                table: "CarStatuses");

            migrationBuilder.DropIndex(
                name: "IX_CarStatuses_EmployeeId",
                table: "CarStatuses");

            migrationBuilder.DropIndex(
                name: "IX_CarStatuses_UnitId",
                table: "CarStatuses");

            migrationBuilder.DropColumn(
                name: "CarId",
                table: "CarStatuses");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "CarStatuses");

            migrationBuilder.DropColumn(
                name: "UnitId",
                table: "CarStatuses");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CarId",
                table: "CarStatuses",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EmployeeId",
                table: "CarStatuses",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UnitId",
                table: "CarStatuses",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CarStatuses_CarId",
                table: "CarStatuses",
                column: "CarId");

            migrationBuilder.CreateIndex(
                name: "IX_CarStatuses_EmployeeId",
                table: "CarStatuses",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_CarStatuses_UnitId",
                table: "CarStatuses",
                column: "UnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_CarStatuses_Cars_CarId",
                table: "CarStatuses",
                column: "CarId",
                principalTable: "Cars",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CarStatuses_Employees_EmployeeId",
                table: "CarStatuses",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CarStatuses_Units_UnitId",
                table: "CarStatuses",
                column: "UnitId",
                principalTable: "Units",
                principalColumn: "UnitId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
