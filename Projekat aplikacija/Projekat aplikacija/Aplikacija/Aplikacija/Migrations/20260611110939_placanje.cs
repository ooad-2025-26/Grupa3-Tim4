using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Aplikacija.Migrations
{
    /// <inheritdoc />
    public partial class placanje : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Placanje_Korisnik_KorisnikId",
                table: "Placanje");

            migrationBuilder.DropForeignKey(
                name: "FK_Placanje_Sesija_SesijaId",
                table: "Placanje");

            migrationBuilder.AddColumn<DateTime>(
                name: "DatumPlacanja",
                table: "Placanje",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "MetodaPlacanja",
                table: "Placanje",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "RezervacijaId",
                table: "Placanje",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Placanje_RezervacijaId",
                table: "Placanje",
                column: "RezervacijaId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Placanje_Korisnik_KorisnikId",
                table: "Placanje",
                column: "KorisnikId",
                principalTable: "Korisnik",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Placanje_Rezervacija_RezervacijaId",
                table: "Placanje",
                column: "RezervacijaId",
                principalTable: "Rezervacija",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Placanje_Sesija_SesijaId",
                table: "Placanje",
                column: "SesijaId",
                principalTable: "Sesija",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Placanje_Korisnik_KorisnikId",
                table: "Placanje");

            migrationBuilder.DropForeignKey(
                name: "FK_Placanje_Rezervacija_RezervacijaId",
                table: "Placanje");

            migrationBuilder.DropForeignKey(
                name: "FK_Placanje_Sesija_SesijaId",
                table: "Placanje");

            migrationBuilder.DropIndex(
                name: "IX_Placanje_RezervacijaId",
                table: "Placanje");

            migrationBuilder.DropColumn(
                name: "DatumPlacanja",
                table: "Placanje");

            migrationBuilder.DropColumn(
                name: "MetodaPlacanja",
                table: "Placanje");

            migrationBuilder.DropColumn(
                name: "RezervacijaId",
                table: "Placanje");

            migrationBuilder.AddForeignKey(
                name: "FK_Placanje_Korisnik_KorisnikId",
                table: "Placanje",
                column: "KorisnikId",
                principalTable: "Korisnik",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Placanje_Sesija_SesijaId",
                table: "Placanje",
                column: "SesijaId",
                principalTable: "Sesija",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
