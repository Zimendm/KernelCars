using Microsoft.EntityFrameworkCore.Migrations;

namespace KernelCars.Migrations
{
    public partial class AddCarType1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarModel_Manufacturers_ManufacturerId",
                table: "CarModel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CarModel",
                table: "CarModel");

            migrationBuilder.RenameTable(
                name: "CarModel",
                newName: "CarModels");

            migrationBuilder.RenameIndex(
                name: "IX_CarModel_ManufacturerId",
                table: "CarModels",
                newName: "IX_CarModels_ManufacturerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CarModels",
                table: "CarModels",
                column: "CarModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_CarModels_Manufacturers_ManufacturerId",
                table: "CarModels",
                column: "ManufacturerId",
                principalTable: "Manufacturers",
                principalColumn: "ManufacturerId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarModels_Manufacturers_ManufacturerId",
                table: "CarModels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CarModels",
                table: "CarModels");

            migrationBuilder.RenameTable(
                name: "CarModels",
                newName: "CarModel");

            migrationBuilder.RenameIndex(
                name: "IX_CarModels_ManufacturerId",
                table: "CarModel",
                newName: "IX_CarModel_ManufacturerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CarModel",
                table: "CarModel",
                column: "CarModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_CarModel_Manufacturers_ManufacturerId",
                table: "CarModel",
                column: "ManufacturerId",
                principalTable: "Manufacturers",
                principalColumn: "ManufacturerId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
