using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WEBTRUYEN.Migrations
{
    /// <inheritdoc />
    public partial class improveReadingHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReadingHistories_AspNetUsers_UsersId",
                table: "ReadingHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_ReadingHistories_Chapters_ChaptersId",
                table: "ReadingHistories");

            migrationBuilder.DropColumn(
                name: "AvatarUrl",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "ChaptersId",
                table: "ReadingHistories",
                newName: "ChapterId");

            migrationBuilder.RenameColumn(
                name: "UsersId",
                table: "ReadingHistories",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_ReadingHistories_ChaptersId",
                table: "ReadingHistories",
                newName: "IX_ReadingHistories_ChapterId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReadingHistories_AspNetUsers_UserId",
                table: "ReadingHistories",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReadingHistories_Chapters_ChapterId",
                table: "ReadingHistories",
                column: "ChapterId",
                principalTable: "Chapters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReadingHistories_AspNetUsers_UserId",
                table: "ReadingHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_ReadingHistories_Chapters_ChapterId",
                table: "ReadingHistories");

            migrationBuilder.RenameColumn(
                name: "ChapterId",
                table: "ReadingHistories",
                newName: "ChaptersId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "ReadingHistories",
                newName: "UsersId");

            migrationBuilder.RenameIndex(
                name: "IX_ReadingHistories_ChapterId",
                table: "ReadingHistories",
                newName: "IX_ReadingHistories_ChaptersId");

            migrationBuilder.AddColumn<string>(
                name: "AvatarUrl",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ReadingHistories_AspNetUsers_UsersId",
                table: "ReadingHistories",
                column: "UsersId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReadingHistories_Chapters_ChaptersId",
                table: "ReadingHistories",
                column: "ChaptersId",
                principalTable: "Chapters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
