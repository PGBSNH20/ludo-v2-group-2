using Microsoft.EntityFrameworkCore.Migrations;

namespace LudoEngine.Migrations
{
    public partial class activePlayer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Boards",
                keyColumn: "Id",
                keyValue: 5,
                column: "activePlayerId",
                value: 1);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Boards",
                keyColumn: "Id",
                keyValue: 5,
                column: "activePlayerId",
                value: 0);
        }
    }
}
