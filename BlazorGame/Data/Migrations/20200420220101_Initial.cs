using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BlazorGame.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "QuizItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Question = table.Column<string>(nullable: false),
                    Answer = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuizItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserStates",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    CurrentScore = table.Column<int>(nullable: false),
                    CanCreate = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserStates", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Completed",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    ItemId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Completed", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Completed_QuizItems_ItemId",
                        column: x => x.ItemId,
                        principalTable: "QuizItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Completed_UserStates_UserId",
                        column: x => x.UserId,
                        principalTable: "UserStates",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Completed_ItemId",
                table: "Completed",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Completed_UserId",
                table: "Completed",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Completed");

            migrationBuilder.DropTable(
                name: "QuizItems");

            migrationBuilder.DropTable(
                name: "UserStates");
        }
    }
}
