using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FizzBuzz.Migrations
{
    /// <inheritdoc />
    public partial class AddGameSessions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "GameId1",
                table: "Sessions",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_GameId1",
                table: "Sessions",
                column: "GameId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Sessions_Games_GameId1",
                table: "Sessions",
                column: "GameId1",
                principalTable: "Games",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sessions_Games_GameId1",
                table: "Sessions");

            migrationBuilder.DropIndex(
                name: "IX_Sessions_GameId1",
                table: "Sessions");

            migrationBuilder.DropColumn(
                name: "GameId1",
                table: "Sessions");
        }
    }
}
