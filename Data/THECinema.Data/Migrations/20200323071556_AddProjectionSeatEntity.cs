using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace THECinema.Data.Migrations
{
    public partial class AddProjectionSeatEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFree",
                table: "Seats");

            migrationBuilder.CreateTable(
                name: "ProjectionsSeats",
                columns: table => new
                {
                    ProjectionId = table.Column<string>(nullable: false),
                    SeatId = table.Column<int>(nullable: false),
                    Id = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsTaken = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectionsSeats", x => new { x.ProjectionId, x.SeatId });
                    table.ForeignKey(
                        name: "FK_ProjectionsSeats_Projections_ProjectionId",
                        column: x => x.ProjectionId,
                        principalTable: "Projections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjectionsSeats_Seats_SeatId",
                        column: x => x.SeatId,
                        principalTable: "Seats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectionsSeats_IsDeleted",
                table: "ProjectionsSeats",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectionsSeats_SeatId",
                table: "ProjectionsSeats",
                column: "SeatId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectionsSeats");

            migrationBuilder.AddColumn<bool>(
                name: "IsFree",
                table: "Seats",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
