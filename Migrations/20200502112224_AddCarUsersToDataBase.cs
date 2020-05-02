using Microsoft.EntityFrameworkCore.Migrations;

namespace KernelCars.Migrations
{
    public partial class AddCarUsersToDataBase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarUser_Cars_CarId",
                table: "CarUser");

            migrationBuilder.DropForeignKey(
                name: "FK_CarUser_Employees_EmployeeId",
                table: "CarUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CarUser",
                table: "CarUser");

            migrationBuilder.RenameTable(
                name: "CarUser",
                newName: "CarUsers");

            migrationBuilder.RenameIndex(
                name: "IX_CarUser_EmployeeId",
                table: "CarUsers",
                newName: "IX_CarUsers_EmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_CarUser_CarId",
                table: "CarUsers",
                newName: "IX_CarUsers_CarId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CarUsers",
                table: "CarUsers",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_CarUsers_Cars_CarId",
                table: "CarUsers",
                column: "CarId",
                principalTable: "Cars",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CarUsers_Employees_EmployeeId",
                table: "CarUsers",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarUsers_Cars_CarId",
                table: "CarUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_CarUsers_Employees_EmployeeId",
                table: "CarUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CarUsers",
                table: "CarUsers");

            migrationBuilder.RenameTable(
                name: "CarUsers",
                newName: "CarUser");

            migrationBuilder.RenameIndex(
                name: "IX_CarUsers_EmployeeId",
                table: "CarUser",
                newName: "IX_CarUser_EmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_CarUsers_CarId",
                table: "CarUser",
                newName: "IX_CarUser_CarId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CarUser",
                table: "CarUser",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_CarUser_Cars_CarId",
                table: "CarUser",
                column: "CarId",
                principalTable: "Cars",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CarUser_Employees_EmployeeId",
                table: "CarUser",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
