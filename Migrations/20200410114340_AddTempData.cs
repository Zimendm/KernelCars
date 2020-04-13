using Microsoft.EntityFrameworkCore.Migrations;

namespace KernelCars.Migrations
{
    public partial class AddTempData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TempModel",
                table: "Cars",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TempOwner",
                table: "Cars",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TempType",
                table: "Cars",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TempUser",
                table: "Cars",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TempModel",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "TempOwner",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "TempType",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "TempUser",
                table: "Cars");
        }
    }
}
