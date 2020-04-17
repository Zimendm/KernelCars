using Microsoft.EntityFrameworkCore.Migrations;

namespace KernelCars.Migrations
{
    public partial class CarStatusAdd5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarStatuses_Cars_CarId1",
                table: "CarStatuses");

            migrationBuilder.DropIndex(
                name: "IX_CarStatuses_CarId1",
                table: "CarStatuses");

            migrationBuilder.DropColumn(
                name: "CarId1",
                table: "CarStatuses");

            migrationBuilder.AlterColumn<long>(
                name: "CarId",
                table: "CarStatuses",
                nullable: false,
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
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarStatuses_Cars_CarId",
                table: "CarStatuses");

            migrationBuilder.DropIndex(
                name: "IX_CarStatuses_CarId",
                table: "CarStatuses");

            migrationBuilder.AlterColumn<int>(
                name: "CarId",
                table: "CarStatuses",
                type: "int",
                nullable: false,
                oldClrType: typeof(long));

            migrationBuilder.AddColumn<long>(
                name: "CarId1",
                table: "CarStatuses",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CarStatuses_CarId1",
                table: "CarStatuses",
                column: "CarId1");

            migrationBuilder.AddForeignKey(
                name: "FK_CarStatuses_Cars_CarId1",
                table: "CarStatuses",
                column: "CarId1",
                principalTable: "Cars",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
