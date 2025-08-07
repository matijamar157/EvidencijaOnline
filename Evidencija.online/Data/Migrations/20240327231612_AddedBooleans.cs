using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Evidencija.online.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedBooleans : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Free",
                table: "Korisnik",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Premium",
                table: "Korisnik",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "PremiumEnds",
                table: "Korisnik",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Free",
                table: "Korisnik");

            migrationBuilder.DropColumn(
                name: "Premium",
                table: "Korisnik");

            migrationBuilder.DropColumn(
                name: "PremiumEnds",
                table: "Korisnik");
        }
    }
}
