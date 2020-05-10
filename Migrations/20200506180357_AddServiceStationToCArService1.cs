using Microsoft.EntityFrameworkCore.Migrations;

namespace KernelCars.Migrations
{
    public partial class AddServiceStationToCArService1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarServices_ServiceStations_ServiceStationID",
                table: "CarServices");

            migrationBuilder.AlterColumn<int>(
                name: "ServiceStationID",
                table: "CarServices",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CarServices_ServiceStations_ServiceStationID",
                table: "CarServices",
                column: "ServiceStationID",
                principalTable: "ServiceStations",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarServices_ServiceStations_ServiceStationID",
                table: "CarServices");

            migrationBuilder.AlterColumn<int>(
                name: "ServiceStationID",
                table: "CarServices",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_CarServices_ServiceStations_ServiceStationID",
                table: "CarServices",
                column: "ServiceStationID",
                principalTable: "ServiceStations",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
