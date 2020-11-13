using Microsoft.EntityFrameworkCore.Migrations;

namespace KernelCars.Migrations
{
    public partial class ClusterAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClusterId",
                table: "Units",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Clusters",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClusterName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clusters", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Units_ClusterId",
                table: "Units",
                column: "ClusterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Units_Clusters_ClusterId",
                table: "Units",
                column: "ClusterId",
                principalTable: "Clusters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Units_Clusters_ClusterId",
                table: "Units");

            migrationBuilder.DropTable(
                name: "Clusters");

            migrationBuilder.DropIndex(
                name: "IX_Units_ClusterId",
                table: "Units");

            migrationBuilder.DropColumn(
                name: "ClusterId",
                table: "Units");
        }
    }
}
