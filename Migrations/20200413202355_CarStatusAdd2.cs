using Microsoft.EntityFrameworkCore.Migrations;

namespace KernelCars.Migrations
{
    public partial class CarStatusAdd2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarStatuses_Cars_CarId1",
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
                name: "IX_CarStatuses_StatusId",
                table: "CarStatuses");

            migrationBuilder.DropColumn(
                name: "CarId1",
                table: "CarStatuses");

            migrationBuilder.DropColumn(
                name: "Comment",
                table: "CarStatuses");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "CarStatuses");

            migrationBuilder.AlterColumn<int>(
                name: "UnitId",
                table: "CarStatuses",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "EmployeeId",
                table: "CarStatuses",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<long>(
                name: "CarId",
                table: "CarStatuses",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_CarStatuses_CarId",
                table: "CarStatuses",
                column: "CarId");

            migrationBuilder.AddForeignKey(
                name: "FK_CarStatuses_Cars_CarId",
                table: "CarStatuses",
                column: "CarId",
                principalTable: "Cars",
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarStatuses_Cars_CarId",
                table: "CarStatuses");

            migrationBuilder.DropForeignKey(
                name: "FK_CarStatuses_Units_UnitId",
                table: "CarStatuses");

            migrationBuilder.DropIndex(
                name: "IX_CarStatuses_CarId",
                table: "CarStatuses");

            migrationBuilder.AlterColumn<int>(
                name: "UnitId",
                table: "CarStatuses",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "EmployeeId",
                table: "CarStatuses",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CarId",
                table: "CarStatuses",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CarId1",
                table: "CarStatuses",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "CarStatuses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StatusId",
                table: "CarStatuses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CarStatuses_CarId1",
                table: "CarStatuses",
                column: "CarId1");

            migrationBuilder.CreateIndex(
                name: "IX_CarStatuses_StatusId",
                table: "CarStatuses",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_CarStatuses_Cars_CarId1",
                table: "CarStatuses",
                column: "CarId1",
                principalTable: "Cars",
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
    }
}
