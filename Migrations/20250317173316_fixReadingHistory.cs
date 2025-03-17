using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WEBTRUYEN.Migrations
{
    /// <inheritdoc />
    public partial class fixReadingHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReadingHistory_AspNetUsers_UsersId",
                table: "ReadingHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_ReadingHistory_Chapters_ChaptersId",
                table: "ReadingHistory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ReadingHistory",
                table: "ReadingHistory");

            migrationBuilder.RenameTable(
                name: "ReadingHistory",
                newName: "ReadingHistories");

            migrationBuilder.RenameIndex(
                name: "IX_ReadingHistory_ChaptersId",
                table: "ReadingHistories",
                newName: "IX_ReadingHistories_ChaptersId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReadingHistories",
                table: "ReadingHistories",
                columns: new[] { "UsersId", "ChaptersId" });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReadingHistories_AspNetUsers_UsersId",
                table: "ReadingHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_ReadingHistories_Chapters_ChaptersId",
                table: "ReadingHistories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ReadingHistories",
                table: "ReadingHistories");

            migrationBuilder.RenameTable(
                name: "ReadingHistories",
                newName: "ReadingHistory");

            migrationBuilder.RenameIndex(
                name: "IX_ReadingHistories_ChaptersId",
                table: "ReadingHistory",
                newName: "IX_ReadingHistory_ChaptersId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReadingHistory",
                table: "ReadingHistory",
                columns: new[] { "UsersId", "ChaptersId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ReadingHistory_AspNetUsers_UsersId",
                table: "ReadingHistory",
                column: "UsersId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReadingHistory_Chapters_ChaptersId",
                table: "ReadingHistory",
                column: "ChaptersId",
                principalTable: "Chapters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
