using Microsoft.EntityFrameworkCore.Migrations;

namespace KernelCars.Migrations
{
    public partial class UpdateManagersTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Managers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Managers");

            migrationBuilder.DropColumn(
                name: "MiddleName",
                table: "Managers");

            migrationBuilder.DropColumn(
                name: "Registration",
                table: "Managers");

            migrationBuilder.AddColumn<int>(
                name: "EmployeeId",
                table: "Managers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Managers_EmployeeId",
                table: "Managers",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Managers_Employees_EmployeeId",
                table: "Managers",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Managers_Employees_EmployeeId",
                table: "Managers");

            migrationBuilder.DropIndex(
                name: "IX_Managers_EmployeeId",
                table: "Managers");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "Managers");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Managers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Managers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MiddleName",
                table: "Managers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Registration",
                table: "Managers",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
