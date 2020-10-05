using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KernelCars.Migrations
{
    public partial class ModifyLeaseContracts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ManagerId",
                table: "LeaseContracts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Manager",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    MiddleName = table.Column<string>(nullable: true),
                    Registration = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    LeaseProcuratoryNumber = table.Column<string>(nullable: true),
                    LeaseProcuratoryDate = table.Column<DateTime>(nullable: false),
                    LeaseProcuratory = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Manager", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LeaseContracts_ManagerId",
                table: "LeaseContracts",
                column: "ManagerId");

            migrationBuilder.AddForeignKey(
                name: "FK_LeaseContracts_Manager_ManagerId",
                table: "LeaseContracts",
                column: "ManagerId",
                principalTable: "Manager",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LeaseContracts_Manager_ManagerId",
                table: "LeaseContracts");

            migrationBuilder.DropTable(
                name: "Manager");

            migrationBuilder.DropIndex(
                name: "IX_LeaseContracts_ManagerId",
                table: "LeaseContracts");

            migrationBuilder.DropColumn(
                name: "ManagerId",
                table: "LeaseContracts");
        }
    }
}
