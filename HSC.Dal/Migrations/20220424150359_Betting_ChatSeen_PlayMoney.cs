using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HSC.Dal.Migrations
{
    public partial class Betting_ChatSeen_PlayMoney : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentBet",
                table: "Match");

            migrationBuilder.AddColumn<bool>(
                name: "IsUsingPlayMoney",
                table: "User",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "CurrentBet",
                table: "MatchPlayer",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "IsBetting",
                table: "MatchPlayer",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSeen",
                table: "ChatMessages",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsUsingPlayMoney",
                table: "User");

            migrationBuilder.DropColumn(
                name: "CurrentBet",
                table: "MatchPlayer");

            migrationBuilder.DropColumn(
                name: "IsBetting",
                table: "MatchPlayer");

            migrationBuilder.DropColumn(
                name: "IsSeen",
                table: "ChatMessages");

            migrationBuilder.AddColumn<decimal>(
                name: "CurrentBet",
                table: "Match",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
