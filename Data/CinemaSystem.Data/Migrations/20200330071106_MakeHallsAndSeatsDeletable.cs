using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CinemaSystem.Data.Migrations
{
    public partial class MakeHallsAndSeatsDeletable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "Seats",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Seats",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "Halls",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Halls",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Seats_IsDeleted",
                table: "Seats",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Halls_IsDeleted",
                table: "Halls",
                column: "IsDeleted");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Seats_IsDeleted",
                table: "Seats");

            migrationBuilder.DropIndex(
                name: "IX_Halls_IsDeleted",
                table: "Halls");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "Seats");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Seats");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "Halls");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Halls");
        }
    }
}
