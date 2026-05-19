using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fiap.Banco.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CAMPEONATOS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    Nome = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    Temporada = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: false),
                    DataInicio = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    DataFim = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    Tipo = table.Column<string>(type: "NVARCHAR2(21)", maxLength: 21, nullable: false),
                    TotalRodadas = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    ConfrontosGerados = table.Column<short>(type: "NUMBER(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CAMPEONATOS", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TIMES",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    Nome = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    Cidade = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    AnoFundacao = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TIMES", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ESTATISTICAS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    TimeId = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    CampeonatoId = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    Pontos = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    Vitorias = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    Derrotas = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    Empates = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    GolsPro = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    GolsContra = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ESTATISTICAS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ESTATISTICAS_CAMPEONATOS_CampeonatoId",
                        column: x => x.CampeonatoId,
                        principalTable: "CAMPEONATOS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ESTATISTICAS_TIMES_TimeId",
                        column: x => x.TimeId,
                        principalTable: "TIMES",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PARTIDAS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    TimeCasaId = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    TimeVisitanteId = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    DataPartida = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    GolsCasa = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    GolsVisitante = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    CampeonatoId = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PARTIDAS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PARTIDAS_CAMPEONATOS_CampeonatoId",
                        column: x => x.CampeonatoId,
                        principalTable: "CAMPEONATOS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PARTIDAS_TIMES_TimeCasaId",
                        column: x => x.TimeCasaId,
                        principalTable: "TIMES",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PARTIDAS_TIMES_TimeVisitanteId",
                        column: x => x.TimeVisitanteId,
                        principalTable: "TIMES",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PESSOAS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    Nome = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    DataNascimento = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    Tipo = table.Column<string>(type: "NVARCHAR2(8)", maxLength: 8, nullable: false),
                    Posicao = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: true),
                    NumeroCamisa = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    TimeId = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    Especialidade = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: true),
                    Tecnico_TimeId = table.Column<int>(type: "NUMBER(10)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PESSOAS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PESSOAS_TIMES_Tecnico_TimeId",
                        column: x => x.Tecnico_TimeId,
                        principalTable: "TIMES",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PESSOAS_TIMES_TimeId",
                        column: x => x.TimeId,
                        principalTable: "TIMES",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TIMES_CAMPEONATOS",
                columns: table => new
                {
                    TimeId = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    CampeonatoId = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TIMES_CAMPEONATOS", x => new { x.TimeId, x.CampeonatoId });
                    table.ForeignKey(
                        name: "FK_TIMES_CAMPEONATOS_CAMPEONATOS_CampeonatoId",
                        column: x => x.CampeonatoId,
                        principalTable: "CAMPEONATOS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TIMES_CAMPEONATOS_TIMES_TimeId",
                        column: x => x.TimeId,
                        principalTable: "TIMES",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TROFEUS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    Nome = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    Ano = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    TimeId = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TROFEUS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TROFEUS_TIMES_TimeId",
                        column: x => x.TimeId,
                        principalTable: "TIMES",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ESTATISTICAS_CampeonatoId",
                table: "ESTATISTICAS",
                column: "CampeonatoId");

            migrationBuilder.CreateIndex(
                name: "IX_ESTATISTICAS_TimeId_CampeonatoId",
                table: "ESTATISTICAS",
                columns: new[] { "TimeId", "CampeonatoId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PARTIDAS_CampeonatoId",
                table: "PARTIDAS",
                column: "CampeonatoId");

            migrationBuilder.CreateIndex(
                name: "IX_PARTIDAS_TimeCasaId",
                table: "PARTIDAS",
                column: "TimeCasaId");

            migrationBuilder.CreateIndex(
                name: "IX_PARTIDAS_TimeVisitanteId",
                table: "PARTIDAS",
                column: "TimeVisitanteId");

            migrationBuilder.CreateIndex(
                name: "IX_PESSOAS_Tecnico_TimeId",
                table: "PESSOAS",
                column: "Tecnico_TimeId");

            migrationBuilder.CreateIndex(
                name: "IX_PESSOAS_TimeId",
                table: "PESSOAS",
                column: "TimeId");

            migrationBuilder.CreateIndex(
                name: "IX_TIMES_CAMPEONATOS_CampeonatoId",
                table: "TIMES_CAMPEONATOS",
                column: "CampeonatoId");

            migrationBuilder.CreateIndex(
                name: "IX_TROFEUS_TimeId",
                table: "TROFEUS",
                column: "TimeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ESTATISTICAS");

            migrationBuilder.DropTable(
                name: "PARTIDAS");

            migrationBuilder.DropTable(
                name: "PESSOAS");

            migrationBuilder.DropTable(
                name: "TIMES_CAMPEONATOS");

            migrationBuilder.DropTable(
                name: "TROFEUS");

            migrationBuilder.DropTable(
                name: "CAMPEONATOS");

            migrationBuilder.DropTable(
                name: "TIMES");
        }
    }
}
