using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WEBTRUYEN.Migrations
{
    /// <inheritdoc />
    public partial class fixReadhingHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chapters_AspNetUsers_UserId",
                table: "Chapters");

            migrationBuilder.DropForeignKey(
                name: "FK_Chapters_Chapters_ChapterId",
                table: "Chapters");

            migrationBuilder.DropIndex(
                name: "IX_Chapters_ChapterId",
                table: "Chapters");

            migrationBuilder.DropIndex(
                name: "IX_Chapters_UserId",
                table: "Chapters");

            migrationBuilder.DropColumn(
                name: "ChapterId",
                table: "Chapters");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Chapters");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChapterUser");

            migrationBuilder.AddColumn<int>(
                name: "ChapterId",
                table: "Chapters",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Chapters",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Chapters_ChapterId",
                table: "Chapters",
                column: "ChapterId");

            migrationBuilder.CreateIndex(
                name: "IX_Chapters_UserId",
                table: "Chapters",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Chapters_AspNetUsers_UserId",
                table: "Chapters",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Chapters_Chapters_ChapterId",
                table: "Chapters",
                column: "ChapterId",
                principalTable: "Chapters",
                principalColumn: "Id");
        }
    }
}
