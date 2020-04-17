using Microsoft.EntityFrameworkCore.Migrations;

namespace KernelCars.Migrations
{
    public partial class CarStatusAdd4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CarId",
                table: "CarStatuses",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<long>(
                name: "CarId1",
                table: "CarStatuses",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "CarStatuses",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EmployeeId",
                table: "CarStatuses",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StatusId",
                table: "CarStatuses",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UnitId",
                table: "CarStatuses",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CarStatuses_CarId1",
                table: "CarStatuses",
                column: "CarId1");

            migrationBuilder.CreateIndex(
                name: "IX_CarStatuses_EmployeeId",
                table: "CarStatuses",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_CarStatuses_StatusId",
                table: "CarStatuses",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_CarStatuses_UnitId",
                table: "CarStatuses",
                column: "UnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_CarStatuses_Cars_CarId1",
                table: "CarStatuses",
                column: "CarId1",
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
                name: "FK_CarStatuses_Statuses_StatusId",
                table: "CarStatuses",
                column: "StatusId",
                principalTable: "Statuses",
                principalColumn: "StatusID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CarStatuses_Units_UnitId",
                table: "CarStatuses",
                column: "UnitId",
                principalTable: "Units",
                principalColumn: "UnitId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarStatuses_Cars_CarId1",
                table: "CarStatuses");

            migrationBuilder.DropForeignKey(
                name: "FK_CarStatuses_Employees_EmployeeId",
                table: "CarStatuses");

            migrationBuilder.DropForeignKey(
                name: "FK_CarStatuses_Statuses_StatusId",
                table: "CarStatuses");

            migrationBuilder.DropForeignKey(
                name: "FK_CarStatuses_Units_UnitId",
                table: "CarStatuses");

            migrationBuilder.DropIndex(
                name: "IX_CarStatuses_CarId1",
                table: "CarStatuses");

            migrationBuilder.DropIndex(
                name: "IX_CarStatuses_EmployeeId",
                table: "CarStatuses");

            migrationBuilder.DropIndex(
                name: "IX_CarStatuses_StatusId",
                table: "CarStatuses");

            migrationBuilder.DropIndex(
                name: "IX_CarStatuses_UnitId",
                table: "CarStatuses");

            migrationBuilder.DropColumn(
                name: "CarId",
                table: "CarStatuses");

            migrationBuilder.DropColumn(
                name: "CarId1",
                table: "CarStatuses");

            migrationBuilder.DropColumn(
                name: "Comment",
                table: "CarStatuses");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "CarStatuses");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "CarStatuses");

            migrationBuilder.DropColumn(
                name: "UnitId",
                table: "CarStatuses");
        }
    }
}
