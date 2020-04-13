using Microsoft.EntityFrameworkCore.Migrations;

namespace KernelCars.Migrations
{
    public partial class ArgiCorrection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAgriBusines",
                table: "Departments");

            migrationBuilder.AddColumn<bool>(
                name: "IsAgriBusiness",
                table: "Units",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAgriBusiness",
                table: "Units");

            migrationBuilder.AddColumn<bool>(
                name: "IsAgriBusines",
                table: "Departments",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
