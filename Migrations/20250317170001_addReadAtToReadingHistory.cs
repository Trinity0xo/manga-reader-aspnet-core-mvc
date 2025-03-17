using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WEBTRUYEN.Migrations
{
    /// <inheritdoc />
    public partial class addReadAtToReadingHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChapterUser");

            migrationBuilder.CreateTable(
                name: "ReadingHistory",
                columns: table => new
                {
                    UsersId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ChaptersId = table.Column<int>(type: "int", nullable: false),
                    ReadAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReadingHistory", x => new { x.UsersId, x.ChaptersId });
                    table.ForeignKey(
                        name: "FK_ReadingHistory_AspNetUsers_UsersId",
                        column: x => x.UsersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReadingHistory_Chapters_ChaptersId",
                        column: x => x.ChaptersId,
                        principalTable: "Chapters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReadingHistory_ChaptersId",
                table: "ReadingHistory",
                column: "ChaptersId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReadingHistory");

            migrationBuilder.CreateTable(
                name: "ChapterUser",
                columns: table => new
                {
                    ChaptersId = table.Column<int>(type: "int", nullable: false),
                    UsersId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChapterUser", x => new { x.ChaptersId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_ChapterUser_AspNetUsers_UsersId",
                        column: x => x.UsersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChapterUser_Chapters_ChaptersId",
                        column: x => x.ChaptersId,
                        principalTable: "Chapters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChapterUser_UsersId",
                table: "ChapterUser",
                column: "UsersId");
        }
    }
}
