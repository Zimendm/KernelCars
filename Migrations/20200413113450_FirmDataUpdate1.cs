using Microsoft.EntityFrameworkCore.Migrations;

namespace KernelCars.Migrations
{
    public partial class FirmDataUpdate1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FirmDepartment_Firms_FirmId",
                table: "FirmDepartment");

            migrationBuilder.DropForeignKey(
                name: "FK_Firms_Employees_EmployeeId1",
                table: "Firms");

            migrationBuilder.DropForeignKey(
                name: "FK_Units_Firms_FirmId",
                table: "Units");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Firms",
                table: "Firms");

            migrationBuilder.DropIndex(
                name: "IX_Firms_EmployeeId1",
                table: "Firms");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "Firms");

            migrationBuilder.DropColumn(
                name: "EmployeeId1",
                table: "Firms");

            migrationBuilder.AddColumn<int>(
                name: "FirmId",
                table: "Firms",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Firms",
                table: "Firms",
                column: "FirmId");

            migrationBuilder.AddForeignKey(
                name: "FK_FirmDepartment_Firms_FirmId",
                table: "FirmDepartment",
                column: "FirmId",
                principalTable: "Firms",
                principalColumn: "FirmId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Units_Firms_FirmId",
                table: "Units",
                column: "FirmId",
                principalTable: "Firms",
                principalColumn: "FirmId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FirmDepartment_Firms_FirmId",
                table: "FirmDepartment");

            migrationBuilder.DropForeignKey(
                name: "FK_Units_Firms_FirmId",
                table: "Units");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Firms",
                table: "Firms");

            migrationBuilder.DropColumn(
                name: "FirmId",
                table: "Firms");

            migrationBuilder.AddColumn<int>(
                name: "EmployeeId",
                table: "Firms",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "EmployeeId1",
                table: "Firms",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Firms",
                table: "Firms",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Firms_EmployeeId1",
                table: "Firms",
                column: "EmployeeId1");

            migrationBuilder.AddForeignKey(
                name: "FK_FirmDepartment_Firms_FirmId",
                table: "FirmDepartment",
                column: "FirmId",
                principalTable: "Firms",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Firms_Employees_EmployeeId1",
                table: "Firms",
                column: "EmployeeId1",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Units_Firms_FirmId",
                table: "Units",
                column: "FirmId",
                principalTable: "Firms",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
