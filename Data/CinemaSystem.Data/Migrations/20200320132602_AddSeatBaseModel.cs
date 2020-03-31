using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CinemaSystem.Data.Migrations
{
    public partial class AddSeatBaseModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Seats",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "Seats",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Seats");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "Seats");
        }
    }
}
