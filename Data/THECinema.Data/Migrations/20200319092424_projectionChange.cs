using Microsoft.EntityFrameworkCore.Migrations;

namespace THECinema.Data.Migrations
{
    public partial class projectionChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Projections_ProjectionHallId_ProjectionMovieId",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_ProjectionHallId_ProjectionMovieId",
                table: "Reservations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Projections",
                table: "Projections");

            migrationBuilder.DropColumn(
                name: "ProjectionHallId",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "ProjectionMovieId",
                table: "Reservations");

            migrationBuilder.AlterColumn<string>(
                name: "ProjectionId",
                table: "Reservations",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "Projections",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Projections",
                table: "Projections",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_ProjectionId",
                table: "Reservations",
                column: "ProjectionId");

            migrationBuilder.CreateIndex(
                name: "IX_Projections_HallId",
                table: "Projections",
                column: "HallId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Projections_ProjectionId",
                table: "Reservations",
                column: "ProjectionId",
                principalTable: "Projections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Projections_ProjectionId",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_ProjectionId",
                table: "Reservations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Projections",
                table: "Projections");

            migrationBuilder.DropIndex(
                name: "IX_Projections_HallId",
                table: "Projections");

            migrationBuilder.AlterColumn<string>(
                name: "ProjectionId",
                table: "Reservations",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProjectionHallId",
                table: "Reservations",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProjectionMovieId",
                table: "Reservations",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "Projections",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Projections",
                table: "Projections",
                columns: new[] { "HallId", "MovieId" });

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_ProjectionHallId_ProjectionMovieId",
                table: "Reservations",
                columns: new[] { "ProjectionHallId", "ProjectionMovieId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Projections_ProjectionHallId_ProjectionMovieId",
                table: "Reservations",
                columns: new[] { "ProjectionHallId", "ProjectionMovieId" },
                principalTable: "Projections",
                principalColumns: new[] { "HallId", "MovieId" },
                onDelete: ReferentialAction.Restrict);
        }
    }
}
