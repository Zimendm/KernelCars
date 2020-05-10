using Microsoft.EntityFrameworkCore.Migrations;

namespace KernelCars.Migrations
{
    public partial class AddServiceStationToCArService : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ServiceStationID",
                table: "CarServices",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CarServices_ServiceStationID",
                table: "CarServices",
                column: "ServiceStationID");

            migrationBuilder.AddForeignKey(
                name: "FK_CarServices_ServiceStations_ServiceStationID",
                table: "CarServices",
                column: "ServiceStationID",
                principalTable: "ServiceStations",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarServices_ServiceStations_ServiceStationID",
                table: "CarServices");

            migrationBuilder.DropIndex(
                name: "IX_CarServices_ServiceStationID",
                table: "CarServices");

            migrationBuilder.DropColumn(
                name: "ServiceStationID",
                table: "CarServices");
        }
    }
}
