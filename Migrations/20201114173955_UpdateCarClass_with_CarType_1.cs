using Microsoft.EntityFrameworkCore.Migrations;

namespace KernelCars.Migrations
{
    public partial class UpdateCarClass_with_CarType_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cars_CarType_CarTypeId",
                table: "Cars");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CarType",
                table: "CarType");

            migrationBuilder.RenameTable(
                name: "CarType",
                newName: "CarTypes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CarTypes",
                table: "CarTypes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Cars_CarTypes_CarTypeId",
                table: "Cars",
                column: "CarTypeId",
                principalTable: "CarTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cars_CarTypes_CarTypeId",
                table: "Cars");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CarTypes",
                table: "CarTypes");

            migrationBuilder.RenameTable(
                name: "CarTypes",
                newName: "CarType");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CarType",
                table: "CarType",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Cars_CarType_CarTypeId",
                table: "Cars",
                column: "CarTypeId",
                principalTable: "CarType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
