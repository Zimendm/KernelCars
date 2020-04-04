using Microsoft.EntityFrameworkCore.Migrations;

namespace KernelCars.Migrations
{
    public partial class AddFirmDep : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Firms_Departments_DepartmentId",
                table: "Firms");

            migrationBuilder.DropIndex(
                name: "IX_Firms_DepartmentId",
                table: "Firms");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "Firms");

            migrationBuilder.CreateTable(
                name: "FirmDepartment",
                columns: table => new
                {
                    FirmId = table.Column<int>(nullable: false),
                    DepartmentId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FirmDepartment", x => new { x.FirmId, x.DepartmentId });
                    table.ForeignKey(
                        name: "FK_FirmDepartment_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "DepartmentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FirmDepartment_Firms_FirmId",
                        column: x => x.FirmId,
                        principalTable: "Firms",
                        principalColumn: "FirmId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FirmDepartment_DepartmentId",
                table: "FirmDepartment",
                column: "DepartmentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FirmDepartment");

            migrationBuilder.AddColumn<int>(
                name: "DepartmentId",
                table: "Firms",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Firms_DepartmentId",
                table: "Firms",
                column: "DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Firms_Departments_DepartmentId",
                table: "Firms",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "DepartmentId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
