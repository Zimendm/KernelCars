using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KernelCars.Migrations
{
    public partial class AddCarService : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CarService",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OpenDate = table.Column<DateTime>(nullable: false),
                    CompleteDate = table.Column<DateTime>(nullable: true),
                    ServiceDescription = table.Column<string>(nullable: true),
                    CarId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarService", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CarService_Cars_CarId",
                        column: x => x.CarId,
                        principalTable: "Cars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TypeOfService",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Service = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeOfService", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "WorkAssigment",
                columns: table => new
                {
                    CarServiceID = table.Column<int>(nullable: false),
                    TypeOfServiceId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkAssigment", x => new { x.TypeOfServiceId, x.CarServiceID });
                    table.ForeignKey(
                        name: "FK_WorkAssigment_CarService_CarServiceID",
                        column: x => x.CarServiceID,
                        principalTable: "CarService",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkAssigment_TypeOfService_TypeOfServiceId",
                        column: x => x.TypeOfServiceId,
                        principalTable: "TypeOfService",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CarService_CarId",
                table: "CarService",
                column: "CarId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkAssigment_CarServiceID",
                table: "WorkAssigment",
                column: "CarServiceID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorkAssigment");

            migrationBuilder.DropTable(
                name: "CarService");

            migrationBuilder.DropTable(
                name: "TypeOfService");
        }
    }
}
