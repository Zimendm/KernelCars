using Microsoft.EntityFrameworkCore.Migrations;

namespace KernelCars.Migrations
{
    public partial class CarStatusAdd67 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EmployeeId",
                table: "CarStatuses",
                type: "int",
                nullable: true);

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
    }
}
