using Microsoft.EntityFrameworkCore.Migrations;

namespace CinemaSystem.Data.Migrations
{
    public partial class ChangeSelectedSeatsType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SeatNumber",
                table: "Reservations");

            migrationBuilder.AddColumn<string>(
                name: "SelectedSeats",
                table: "Reservations",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SelectedSeats",
                table: "Reservations");

            migrationBuilder.AddColumn<int>(
                name: "SeatNumber",
                table: "Reservations",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
