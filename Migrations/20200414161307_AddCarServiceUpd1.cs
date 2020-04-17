using Microsoft.EntityFrameworkCore.Migrations;

namespace KernelCars.Migrations
{
    public partial class AddCarServiceUpd1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarService_Cars_CarId",
                table: "CarService");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkAssigment_CarService_CarServiceID",
                table: "WorkAssigment");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkAssigment_TypeOfService_TypeOfServiceId",
                table: "WorkAssigment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TypeOfService",
                table: "TypeOfService");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CarService",
                table: "CarService");

            migrationBuilder.RenameTable(
                name: "TypeOfService",
                newName: "TypeOfServices");

            migrationBuilder.RenameTable(
                name: "CarService",
                newName: "CarServices");

            migrationBuilder.RenameIndex(
                name: "IX_CarService_CarId",
                table: "CarServices",
                newName: "IX_CarServices_CarId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TypeOfServices",
                table: "TypeOfServices",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CarServices",
                table: "CarServices",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_CarServices_Cars_CarId",
                table: "CarServices",
                column: "CarId",
                principalTable: "Cars",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkAssigment_CarServices_CarServiceID",
                table: "WorkAssigment",
                column: "CarServiceID",
                principalTable: "CarServices",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkAssigment_TypeOfServices_TypeOfServiceId",
                table: "WorkAssigment",
                column: "TypeOfServiceId",
                principalTable: "TypeOfServices",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarServices_Cars_CarId",
                table: "CarServices");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkAssigment_CarServices_CarServiceID",
                table: "WorkAssigment");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkAssigment_TypeOfServices_TypeOfServiceId",
                table: "WorkAssigment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TypeOfServices",
                table: "TypeOfServices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CarServices",
                table: "CarServices");

            migrationBuilder.RenameTable(
                name: "TypeOfServices",
                newName: "TypeOfService");

            migrationBuilder.RenameTable(
                name: "CarServices",
                newName: "CarService");

            migrationBuilder.RenameIndex(
                name: "IX_CarServices_CarId",
                table: "CarService",
                newName: "IX_CarService_CarId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TypeOfService",
                table: "TypeOfService",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CarService",
                table: "CarService",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_CarService_Cars_CarId",
                table: "CarService",
                column: "CarId",
                principalTable: "Cars",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkAssigment_CarService_CarServiceID",
                table: "WorkAssigment",
                column: "CarServiceID",
                principalTable: "CarService",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkAssigment_TypeOfService_TypeOfServiceId",
                table: "WorkAssigment",
                column: "TypeOfServiceId",
                principalTable: "TypeOfService",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
