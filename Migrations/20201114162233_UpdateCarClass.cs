using Microsoft.EntityFrameworkCore.Migrations;

namespace KernelCars.Migrations
{
    public partial class UpdateCarClass : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Units_Clusters_ClusterId",
                table: "Units");

            migrationBuilder.DropColumn(
                name: "CarType",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "Tyres",
                table: "Cars");

            migrationBuilder.AlterColumn<int>(
                name: "ClusterId",
                table: "Units",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DriveType",
                table: "Cars",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "SummerTyres",
                table: "Cars",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Transmission",
                table: "Cars",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "WinterTyres",
                table: "Cars",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Units_Clusters_ClusterId",
                table: "Units",
                column: "ClusterId",
                principalTable: "Clusters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Units_Clusters_ClusterId",
                table: "Units");

            migrationBuilder.DropColumn(
                name: "DriveType",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "SummerTyres",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "Transmission",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "WinterTyres",
                table: "Cars");

            migrationBuilder.AlterColumn<int>(
                name: "ClusterId",
                table: "Units",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<string>(
                name: "CarType",
                table: "Cars",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Tyres",
                table: "Cars",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Units_Clusters_ClusterId",
                table: "Units",
                column: "ClusterId",
                principalTable: "Clusters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
