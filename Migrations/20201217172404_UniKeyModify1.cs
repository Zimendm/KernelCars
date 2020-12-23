using Microsoft.EntityFrameworkCore.Migrations;

namespace KernelCars.Migrations
{
    public partial class UniKeyModify1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddUniqueConstraint(
                name: "AK_Units_DepartmentId_FirmId_ClusterId",
                table: "Units",
                columns: new[] { "DepartmentId", "FirmId", "ClusterId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_Units_DepartmentId_FirmId_ClusterId",
                table: "Units");
        }
    }
}
