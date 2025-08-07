using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Evidencija.online.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Korisnik",
                columns: table => new
                {
                    Ime = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Prezime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Korisnik", x => x.Ime);
                });

            migrationBuilder.CreateTable(
                name: "Trosak",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Opis = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    KorisnikId = table.Column<int>(type: "int", nullable: false),
                    KorisnikIme = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Datum = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Iznos = table.Column<int>(type: "int", nullable: false),
                    Valuta = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trosak", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Trosak_Korisnik_KorisnikIme",
                        column: x => x.KorisnikIme,
                        principalTable: "Korisnik",
                        principalColumn: "Ime",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Kategorija",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Opis = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TrosakId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kategorija", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Kategorija_Trosak_TrosakId",
                        column: x => x.TrosakId,
                        principalTable: "Trosak",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Kategorija_TrosakId",
                table: "Kategorija",
                column: "TrosakId");

            migrationBuilder.CreateIndex(
                name: "IX_Trosak_KorisnikIme",
                table: "Trosak",
                column: "KorisnikIme");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Kategorija");

            migrationBuilder.DropTable(
                name: "Trosak");

            migrationBuilder.DropTable(
                name: "Korisnik");
        }
    }
}
