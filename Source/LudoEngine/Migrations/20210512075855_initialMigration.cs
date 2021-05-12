using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LudoEngine.Migrations
{
    public partial class initialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Boards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LastTimePlayed = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsFinished = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Boards", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Colors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ColorCode = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Colors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ColorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Players_Colors_ColorId",
                        column: x => x.ColorId,
                        principalTable: "Colors",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BoardStates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlayerId = table.Column<int>(type: "int", nullable: false),
                    BoardId = table.Column<int>(type: "int", nullable: false),
                    PieceNumber = table.Column<int>(type: "int", nullable: false),
                    PiecePosition = table.Column<int>(type: "int", nullable: false),
                    IsInSafeZone = table.Column<bool>(type: "bit", nullable: false),
                    IsInBase = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoardStates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BoardStates_Boards_BoardId",
                        column: x => x.BoardId,
                        principalTable: "Boards",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BoardStates_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Winners",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlayerId = table.Column<int>(type: "int", nullable: false),
                    BoardId = table.Column<int>(type: "int", nullable: false),
                    Placement = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Winners", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Winners_Boards_BoardId",
                        column: x => x.BoardId,
                        principalTable: "Boards",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Winners_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Boards",
                columns: new[] { "Id", "IsFinished", "LastTimePlayed" },
                values: new object[,]
                {
                    { 1, true, new DateTime(2021, 3, 29, 21, 55, 5, 0, DateTimeKind.Unspecified) },
                    { 2, true, new DateTime(2021, 1, 15, 21, 55, 5, 0, DateTimeKind.Unspecified) },
                    { 3, true, new DateTime(2021, 5, 2, 21, 55, 5, 0, DateTimeKind.Unspecified) },
                    { 4, true, new DateTime(2021, 2, 25, 21, 55, 5, 0, DateTimeKind.Unspecified) },
                    { 5, false, new DateTime(2021, 3, 21, 21, 55, 5, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "Colors",
                columns: new[] { "Id", "ColorCode", "Name" },
                values: new object[,]
                {
                    { 1, 14, "Yellow" },
                    { 2, 12, "Red" },
                    { 3, 9, "Blue" },
                    { 4, 10, "Green" }
                });

            migrationBuilder.InsertData(
                table: "Players",
                columns: new[] { "Id", "ColorId", "Name" },
                values: new object[,]
                {
                    { 2, 1, "Bobby" },
                    { 5, 1, "Roseline" },
                    { 1, 2, "Lisa" },
                    { 6, 2, "Noa" },
                    { 3, 3, "Anna" },
                    { 4, 4, "Luke" }
                });

            migrationBuilder.InsertData(
                table: "BoardStates",
                columns: new[] { "Id", "BoardId", "IsInBase", "IsInSafeZone", "PieceNumber", "PiecePosition", "PlayerId" },
                values: new object[,]
                {
                    { 5, 5, false, true, 1, 0, 2 },
                    { 6, 5, false, false, 2, 22, 2 },
                    { 7, 5, true, false, 3, 0, 2 },
                    { 8, 5, true, false, 4, 0, 2 },
                    { 1, 5, false, false, 1, 18, 1 },
                    { 2, 5, false, false, 2, 13, 1 },
                    { 3, 5, true, false, 3, 0, 1 },
                    { 4, 5, true, false, 4, 0, 1 }
                });

            migrationBuilder.InsertData(
                table: "Winners",
                columns: new[] { "Id", "BoardId", "Placement", "PlayerId" },
                values: new object[,]
                {
                    { 1, 1, 1, 2 },
                    { 6, 2, 1, 5 },
                    { 2, 1, 2, 1 },
                    { 5, 2, 4, 6 },
                    { 3, 2, 2, 3 },
                    { 4, 2, 3, 4 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_BoardStates_BoardId",
                table: "BoardStates",
                column: "BoardId");

            migrationBuilder.CreateIndex(
                name: "IX_BoardStates_PlayerId",
                table: "BoardStates",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Players_ColorId",
                table: "Players",
                column: "ColorId");

            migrationBuilder.CreateIndex(
                name: "IX_Winners_BoardId",
                table: "Winners",
                column: "BoardId");

            migrationBuilder.CreateIndex(
                name: "IX_Winners_PlayerId",
                table: "Winners",
                column: "PlayerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BoardStates");

            migrationBuilder.DropTable(
                name: "Winners");

            migrationBuilder.DropTable(
                name: "Boards");

            migrationBuilder.DropTable(
                name: "Players");

            migrationBuilder.DropTable(
                name: "Colors");
        }
    }
}
