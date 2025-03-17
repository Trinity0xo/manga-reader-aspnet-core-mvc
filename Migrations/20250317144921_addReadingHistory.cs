using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WEBTRUYEN.Migrations
{
    /// <inheritdoc />
    public partial class addReadingHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
