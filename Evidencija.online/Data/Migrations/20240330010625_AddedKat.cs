using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Evidencija.online.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedKat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Kategorija_Trosak_TrosakId",
                table: "Kategorija");

            migrationBuilder.DropIndex(
                name: "IX_Kategorija_TrosakId",
                table: "Kategorija");

            migrationBuilder.DropColumn(
                name: "TrosakId",
                table: "Kategorija");

            migrationBuilder.AddColumn<int>(
                name: "KategorijaId",
                table: "Trosak",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Trosak_KategorijaId",
                table: "Trosak",
                column: "KategorijaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Trosak_Kategorija_KategorijaId",
                table: "Trosak",
                column: "KategorijaId",
                principalTable: "Kategorija",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trosak_Kategorija_KategorijaId",
                table: "Trosak");

            migrationBuilder.DropIndex(
                name: "IX_Trosak_KategorijaId",
                table: "Trosak");

            migrationBuilder.DropColumn(
                name: "KategorijaId",
                table: "Trosak");

            migrationBuilder.AddColumn<int>(
                name: "TrosakId",
                table: "Kategorija",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Kategorija_TrosakId",
                table: "Kategorija",
                column: "TrosakId");

            migrationBuilder.AddForeignKey(
                name: "FK_Kategorija_Trosak_TrosakId",
                table: "Kategorija",
                column: "TrosakId",
                principalTable: "Trosak",
                principalColumn: "Id");
        }
    }
}
