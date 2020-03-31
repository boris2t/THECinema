using Microsoft.EntityFrameworkCore.Migrations;

namespace THECinema.Data.Migrations
{
    public partial class AddReservationToProjSeats : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ReservationId",
                table: "ProjectionsSeats",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectionsSeats_ReservationId",
                table: "ProjectionsSeats",
                column: "ReservationId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectionsSeats_Reservations_ReservationId",
                table: "ProjectionsSeats",
                column: "ReservationId",
                principalTable: "Reservations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectionsSeats_Reservations_ReservationId",
                table: "ProjectionsSeats");

            migrationBuilder.DropIndex(
                name: "IX_ProjectionsSeats_ReservationId",
                table: "ProjectionsSeats");

            migrationBuilder.DropColumn(
                name: "ReservationId",
                table: "ProjectionsSeats");
        }
    }
}
