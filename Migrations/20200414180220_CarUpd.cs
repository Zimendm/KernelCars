using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KernelCars.Migrations
{
    public partial class CarUpd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "ImagePage2",
                table: "Cars",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "LPG",
                table: "Cars",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Tyres",
                table: "Cars",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePage2",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "LPG",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "Tyres",
                table: "Cars");
        }
    }
}
