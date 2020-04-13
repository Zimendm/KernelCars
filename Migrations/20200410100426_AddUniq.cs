using Microsoft.EntityFrameworkCore.Migrations;

namespace KernelCars.Migrations
{
    public partial class AddUniq : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Units_DepartmentId",
                table: "Units");

            migrationBuilder.CreateIndex(
                name: "IX_Units_DepartmentId_FirmId",
                table: "Units",
                columns: new[] { "DepartmentId", "FirmId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Units_DepartmentId_FirmId",
                table: "Units");

            migrationBuilder.CreateIndex(
                name: "IX_Units_DepartmentId",
                table: "Units",
                column: "DepartmentId");
        }
    }
}
