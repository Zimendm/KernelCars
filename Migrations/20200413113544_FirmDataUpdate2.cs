using Microsoft.EntityFrameworkCore.Migrations;

namespace KernelCars.Migrations
{
    public partial class FirmDataUpdate2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EmployeeId",
                table: "Firms",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Firms_EmployeeId",
                table: "Firms",
                column: "EmployeeId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Firms_Employees_EmployeeId",
                table: "Firms",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Firms_Employees_EmployeeId",
                table: "Firms");

            migrationBuilder.DropIndex(
                name: "IX_Firms_EmployeeId",
                table: "Firms");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "Firms");
        }
    }
}
