using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HSC.Dal.Migrations
{
    public partial class Analysis : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Analysis",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MatchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AnalysedGame = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Analysis", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Match_Analysis",
                        column: x => x.MatchId,
                        principalTable: "Match",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Analysis_MatchId",
                table: "Analysis",
                column: "MatchId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Analysis");
        }
    }
}
