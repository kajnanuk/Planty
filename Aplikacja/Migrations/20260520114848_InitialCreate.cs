using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Aplikacja.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Uzytkownicy",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NazwaUzytkownika = table.Column<string>(type: "TEXT", nullable: false),
                    HasloHash = table.Column<string>(type: "TEXT", nullable: false),
                    Rola = table.Column<string>(type: "TEXT", nullable: false),
                    ApiToken = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Uzytkownicy", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Warstwy",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nazwa = table.Column<string>(type: "TEXT", nullable: false),
                    Opis = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Warstwy", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Rosliny",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NazwaPolska = table.Column<string>(type: "TEXT", nullable: false),
                    NazwaLacinska = table.Column<string>(type: "TEXT", nullable: false),
                    Opis = table.Column<string>(type: "TEXT", nullable: false),
                    WarstwaId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rosliny", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rosliny_Warstwy_WarstwaId",
                        column: x => x.WarstwaId,
                        principalTable: "Warstwy",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OgrodyUzytkownikow",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UzytkownikId = table.Column<int>(type: "INTEGER", nullable: true),
                    RoslinaId = table.Column<int>(type: "INTEGER", nullable: true),
                    DataPosadzenia = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IloscSztuk = table.Column<int>(type: "INTEGER", nullable: false),
                    WlasneNotatki = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OgrodyUzytkownikow", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OgrodyUzytkownikow_Rosliny_RoslinaId",
                        column: x => x.RoslinaId,
                        principalTable: "Rosliny",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OgrodyUzytkownikow_Uzytkownicy_UzytkownikId",
                        column: x => x.UzytkownikId,
                        principalTable: "Uzytkownicy",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Wymagania",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RoslinaId = table.Column<int>(type: "INTEGER", nullable: true),
                    Naslonecznienie = table.Column<string>(type: "TEXT", nullable: false),
                    RodzajGleby = table.Column<string>(type: "TEXT", nullable: false),
                    ZapotrzebowanieNaWode = table.Column<string>(type: "TEXT", nullable: false),
                    Mrozoodpornosc = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wymagania", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Wymagania_Rosliny_RoslinaId",
                        column: x => x.RoslinaId,
                        principalTable: "Rosliny",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Zaleznosci",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RoslinaGlownaId = table.Column<int>(type: "INTEGER", nullable: true),
                    RoslinaTowarzyszacaId = table.Column<int>(type: "INTEGER", nullable: true),
                    TypZaleznosci = table.Column<string>(type: "TEXT", nullable: false),
                    Opis = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Zaleznosci", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Zaleznosci_Rosliny_RoslinaGlownaId",
                        column: x => x.RoslinaGlownaId,
                        principalTable: "Rosliny",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Zaleznosci_Rosliny_RoslinaTowarzyszacaId",
                        column: x => x.RoslinaTowarzyszacaId,
                        principalTable: "Rosliny",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OgrodyUzytkownikow_RoslinaId",
                table: "OgrodyUzytkownikow",
                column: "RoslinaId");

            migrationBuilder.CreateIndex(
                name: "IX_OgrodyUzytkownikow_UzytkownikId",
                table: "OgrodyUzytkownikow",
                column: "UzytkownikId");

            migrationBuilder.CreateIndex(
                name: "IX_Rosliny_WarstwaId",
                table: "Rosliny",
                column: "WarstwaId");

            migrationBuilder.CreateIndex(
                name: "IX_Wymagania_RoslinaId",
                table: "Wymagania",
                column: "RoslinaId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Zaleznosci_RoslinaGlownaId",
                table: "Zaleznosci",
                column: "RoslinaGlownaId");

            migrationBuilder.CreateIndex(
                name: "IX_Zaleznosci_RoslinaTowarzyszacaId",
                table: "Zaleznosci",
                column: "RoslinaTowarzyszacaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OgrodyUzytkownikow");

            migrationBuilder.DropTable(
                name: "Wymagania");

            migrationBuilder.DropTable(
                name: "Zaleznosci");

            migrationBuilder.DropTable(
                name: "Uzytkownicy");

            migrationBuilder.DropTable(
                name: "Rosliny");

            migrationBuilder.DropTable(
                name: "Warstwy");
        }
    }
}
