using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HSC.Dal.Migrations
{
    public partial class Light_Theme : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "LightTheme",
                table: "User",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LightTheme",
                table: "User");
        }
    }
}
