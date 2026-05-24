using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Aplikacija.Migrations
{
    /// <inheritdoc />
    public partial class Connections : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Kvar_Uredjaj_UredjajId",
                table: "Kvar");

            migrationBuilder.DropForeignKey(
                name: "FK_Sesija_Uredjaj_UredjajId",
                table: "Sesija");

            migrationBuilder.AddColumn<int>(
                name: "TakmicenjeId",
                table: "SistemZaTakmicenje",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "KorisnikId",
                table: "Sesija",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "TakmicenjeId",
                table: "Sesija",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UredjajId",
                table: "Rezervacija",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SesijaId",
                table: "Placanje",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SesijaId",
                table: "Narudzba",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SesijaId",
                table: "Kvar",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_SistemZaTakmicenje_TakmicenjeId",
                table: "SistemZaTakmicenje",
                column: "TakmicenjeId");

            migrationBuilder.CreateIndex(
                name: "IX_Sesija_KorisnikId",
                table: "Sesija",
                column: "KorisnikId");

            migrationBuilder.CreateIndex(
                name: "IX_Sesija_TakmicenjeId",
                table: "Sesija",
                column: "TakmicenjeId");

            migrationBuilder.CreateIndex(
                name: "IX_Rezervacija_UredjajId",
                table: "Rezervacija",
                column: "UredjajId");

            migrationBuilder.CreateIndex(
                name: "IX_Placanje_SesijaId",
                table: "Placanje",
                column: "SesijaId");

            migrationBuilder.CreateIndex(
                name: "IX_Narudzba_SesijaId",
                table: "Narudzba",
                column: "SesijaId");

            migrationBuilder.CreateIndex(
                name: "IX_Kvar_SesijaId",
                table: "Kvar",
                column: "SesijaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Kvar_Sesija_SesijaId",
                table: "Kvar",
                column: "SesijaId",
                principalTable: "Sesija",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Kvar_Uredjaj_UredjajId",
                table: "Kvar",
                column: "UredjajId",
                principalTable: "Uredjaj",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Narudzba_Sesija_SesijaId",
                table: "Narudzba",
                column: "SesijaId",
                principalTable: "Sesija",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Placanje_Sesija_SesijaId",
                table: "Placanje",
                column: "SesijaId",
                principalTable: "Sesija",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rezervacija_Uredjaj_UredjajId",
                table: "Rezervacija",
                column: "UredjajId",
                principalTable: "Uredjaj",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sesija_Korisnik_KorisnikId",
                table: "Sesija",
                column: "KorisnikId",
                principalTable: "Korisnik",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Sesija_Takmicenje_TakmicenjeId",
                table: "Sesija",
                column: "TakmicenjeId",
                principalTable: "Takmicenje",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Sesija_Uredjaj_UredjajId",
                table: "Sesija",
                column: "UredjajId",
                principalTable: "Uredjaj",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SistemZaTakmicenje_Takmicenje_TakmicenjeId",
                table: "SistemZaTakmicenje",
                column: "TakmicenjeId",
                principalTable: "Takmicenje",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Kvar_Sesija_SesijaId",
                table: "Kvar");

            migrationBuilder.DropForeignKey(
                name: "FK_Kvar_Uredjaj_UredjajId",
                table: "Kvar");

            migrationBuilder.DropForeignKey(
                name: "FK_Narudzba_Sesija_SesijaId",
                table: "Narudzba");

            migrationBuilder.DropForeignKey(
                name: "FK_Placanje_Sesija_SesijaId",
                table: "Placanje");

            migrationBuilder.DropForeignKey(
                name: "FK_Rezervacija_Uredjaj_UredjajId",
                table: "Rezervacija");

            migrationBuilder.DropForeignKey(
                name: "FK_Sesija_Korisnik_KorisnikId",
                table: "Sesija");

            migrationBuilder.DropForeignKey(
                name: "FK_Sesija_Takmicenje_TakmicenjeId",
                table: "Sesija");

            migrationBuilder.DropForeignKey(
                name: "FK_Sesija_Uredjaj_UredjajId",
                table: "Sesija");

            migrationBuilder.DropForeignKey(
                name: "FK_SistemZaTakmicenje_Takmicenje_TakmicenjeId",
                table: "SistemZaTakmicenje");

            migrationBuilder.DropIndex(
                name: "IX_SistemZaTakmicenje_TakmicenjeId",
                table: "SistemZaTakmicenje");

            migrationBuilder.DropIndex(
                name: "IX_Sesija_KorisnikId",
                table: "Sesija");

            migrationBuilder.DropIndex(
                name: "IX_Sesija_TakmicenjeId",
                table: "Sesija");

            migrationBuilder.DropIndex(
                name: "IX_Rezervacija_UredjajId",
                table: "Rezervacija");

            migrationBuilder.DropIndex(
                name: "IX_Placanje_SesijaId",
                table: "Placanje");

            migrationBuilder.DropIndex(
                name: "IX_Narudzba_SesijaId",
                table: "Narudzba");

            migrationBuilder.DropIndex(
                name: "IX_Kvar_SesijaId",
                table: "Kvar");

            migrationBuilder.DropColumn(
                name: "TakmicenjeId",
                table: "SistemZaTakmicenje");

            migrationBuilder.DropColumn(
                name: "KorisnikId",
                table: "Sesija");

            migrationBuilder.DropColumn(
                name: "TakmicenjeId",
                table: "Sesija");

            migrationBuilder.DropColumn(
                name: "UredjajId",
                table: "Rezervacija");

            migrationBuilder.DropColumn(
                name: "SesijaId",
                table: "Placanje");

            migrationBuilder.DropColumn(
                name: "SesijaId",
                table: "Narudzba");

            migrationBuilder.DropColumn(
                name: "SesijaId",
                table: "Kvar");

            migrationBuilder.AddForeignKey(
                name: "FK_Kvar_Uredjaj_UredjajId",
                table: "Kvar",
                column: "UredjajId",
                principalTable: "Uredjaj",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sesija_Uredjaj_UredjajId",
                table: "Sesija",
                column: "UredjajId",
                principalTable: "Uredjaj",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
