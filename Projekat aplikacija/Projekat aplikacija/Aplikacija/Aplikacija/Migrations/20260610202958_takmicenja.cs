using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Aplikacija.Migrations
{
    /// <inheritdoc />
    public partial class takmicenja : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SistemZaTakmicenje_Takmicenje_TakmicenjeId",
                table: "SistemZaTakmicenje");

            migrationBuilder.DropIndex(
                name: "IX_SistemZaTakmicenje_TakmicenjeId",
                table: "SistemZaTakmicenje");

            migrationBuilder.DropColumn(
                name: "TakmicenjeId",
                table: "SistemZaTakmicenje");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TakmicenjeId",
                table: "SistemZaTakmicenje",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_SistemZaTakmicenje_TakmicenjeId",
                table: "SistemZaTakmicenje",
                column: "TakmicenjeId");

            migrationBuilder.AddForeignKey(
                name: "FK_SistemZaTakmicenje_Takmicenje_TakmicenjeId",
                table: "SistemZaTakmicenje",
                column: "TakmicenjeId",
                principalTable: "Takmicenje",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
