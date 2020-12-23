using Microsoft.EntityFrameworkCore.Migrations;

namespace KernelCars.Migrations
{
    public partial class UpdateCarClass1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SummerTyres",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "WinterTyres",
                table: "Cars");

            migrationBuilder.AlterColumn<int>(
                name: "Transmission",
                table: "Cars",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "DriveType",
                table: "Cars",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "TyresId",
                table: "Cars",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TireSizes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Size = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TireSizes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cars_TyresId",
                table: "Cars",
                column: "TyresId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cars_TireSizes_TyresId",
                table: "Cars",
                column: "TyresId",
                principalTable: "TireSizes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cars_TireSizes_TyresId",
                table: "Cars");

            migrationBuilder.DropTable(
                name: "TireSizes");

            migrationBuilder.DropIndex(
                name: "IX_Cars_TyresId",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "TyresId",
                table: "Cars");

            migrationBuilder.AlterColumn<int>(
                name: "Transmission",
                table: "Cars",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DriveType",
                table: "Cars",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SummerTyres",
                table: "Cars",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WinterTyres",
                table: "Cars",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
