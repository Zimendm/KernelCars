using Microsoft.EntityFrameworkCore.Migrations;

namespace KernelCars.Migrations
{
    public partial class AddManagersTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LeaseContracts_Manager_ManagerId",
                table: "LeaseContracts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Manager",
                table: "Manager");

            migrationBuilder.RenameTable(
                name: "Manager",
                newName: "Managers");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Managers",
                table: "Managers",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_LeaseContracts_Managers_ManagerId",
                table: "LeaseContracts",
                column: "ManagerId",
                principalTable: "Managers",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LeaseContracts_Managers_ManagerId",
                table: "LeaseContracts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Managers",
                table: "Managers");

            migrationBuilder.RenameTable(
                name: "Managers",
                newName: "Manager");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Manager",
                table: "Manager",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_LeaseContracts_Manager_ManagerId",
                table: "LeaseContracts",
                column: "ManagerId",
                principalTable: "Manager",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
