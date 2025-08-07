using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Evidencija.online.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trosak_Korisnik_KorisnikIme",
                table: "Trosak");

            migrationBuilder.DropIndex(
                name: "IX_Trosak_KorisnikIme",
                table: "Trosak");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Korisnik",
                table: "Korisnik");

            migrationBuilder.DropColumn(
                name: "KorisnikIme",
                table: "Trosak");

            migrationBuilder.AlterColumn<string>(
                name: "Ime",
                table: "Korisnik",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Korisnik",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Korisnik",
                table: "Korisnik",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Trosak_KorisnikId",
                table: "Trosak",
                column: "KorisnikId");

            migrationBuilder.AddForeignKey(
                name: "FK_Trosak_Korisnik_KorisnikId",
                table: "Trosak",
                column: "KorisnikId",
                principalTable: "Korisnik",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trosak_Korisnik_KorisnikId",
                table: "Trosak");

            migrationBuilder.DropIndex(
                name: "IX_Trosak_KorisnikId",
                table: "Trosak");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Korisnik",
                table: "Korisnik");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Korisnik");

            migrationBuilder.AddColumn<string>(
                name: "KorisnikIme",
                table: "Trosak",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Ime",
                table: "Korisnik",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Korisnik",
                table: "Korisnik",
                column: "Ime");

            migrationBuilder.CreateIndex(
                name: "IX_Trosak_KorisnikIme",
                table: "Trosak",
                column: "KorisnikIme");

            migrationBuilder.AddForeignKey(
                name: "FK_Trosak_Korisnik_KorisnikIme",
                table: "Trosak",
                column: "KorisnikIme",
                principalTable: "Korisnik",
                principalColumn: "Ime",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
