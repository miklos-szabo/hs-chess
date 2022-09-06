using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HSC.Dal.Migrations
{
    public partial class tournaments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSearching",
                table: "TournamentPlayer",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "TournamentStatus",
                table: "Tournament",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSearching",
                table: "TournamentPlayer");

            migrationBuilder.DropColumn(
                name: "TournamentStatus",
                table: "Tournament");
        }
    }
}
