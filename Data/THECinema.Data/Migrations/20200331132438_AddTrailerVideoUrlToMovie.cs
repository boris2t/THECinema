using Microsoft.EntityFrameworkCore.Migrations;

namespace THECinema.Data.Migrations
{
    public partial class AddTrailerVideoUrlToMovie : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TrailerVideoUrl",
                table: "Movies",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TrailerVideoUrl",
                table: "Movies");
        }
    }
}
