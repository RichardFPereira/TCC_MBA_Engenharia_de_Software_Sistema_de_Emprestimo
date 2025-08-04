using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.UsuarioService.Migrations
{
    /// <inheritdoc />
    public partial class AddEmprestimoTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Credenciais_Usuarios_UsuarioId1",
                table: "Credenciais");

            migrationBuilder.DropIndex(
                name: "IX_Credenciais_UsuarioId1",
                table: "Credenciais");

            migrationBuilder.DropColumn(
                name: "UsuarioId1",
                table: "Credenciais");

            migrationBuilder.CreateTable(
                name: "Configuracoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TaxaJuros = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MinParcelas = table.Column<int>(type: "int", nullable: false),
                    MaxParcelas = table.Column<int>(type: "int", nullable: false),
                    PercentualReserva = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configuracoes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Emprestimos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    Valor = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ValorTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NumeroParcelas = table.Column<int>(type: "int", nullable: false),
                    TaxaJuros = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DataEmprestimo = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataCadastro = table.Column<DateOnly>(type: "date", nullable: false),
                    DataAlteracao = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Emprestimos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Emprestimos_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LogAcoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdministradorId = table.Column<int>(type: "int", nullable: false),
                    Acao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Detalhes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Data = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogAcoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LogAcoes_Usuarios_AdministradorId",
                        column: x => x.AdministradorId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Parcelas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmprestimoId = table.Column<int>(type: "int", nullable: false),
                    NumeroParcela = table.Column<int>(type: "int", nullable: false),
                    ValorParcela = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DataVencimento = table.Column<DateOnly>(type: "date", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataPagamento = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parcelas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Parcelas_Emprestimos_EmprestimoId",
                        column: x => x.EmprestimoId,
                        principalTable: "Emprestimos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Emprestimos_UsuarioId",
                table: "Emprestimos",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_LogAcoes_AdministradorId",
                table: "LogAcoes",
                column: "AdministradorId");

            migrationBuilder.CreateIndex(
                name: "IX_Parcelas_EmprestimoId",
                table: "Parcelas",
                column: "EmprestimoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Configuracoes");

            migrationBuilder.DropTable(
                name: "LogAcoes");

            migrationBuilder.DropTable(
                name: "Parcelas");

            migrationBuilder.DropTable(
                name: "Emprestimos");

            migrationBuilder.AddColumn<int>(
                name: "UsuarioId1",
                table: "Credenciais",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Credenciais_UsuarioId1",
                table: "Credenciais",
                column: "UsuarioId1",
                unique: true,
                filter: "[UsuarioId1] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Credenciais_Usuarios_UsuarioId1",
                table: "Credenciais",
                column: "UsuarioId1",
                principalTable: "Usuarios",
                principalColumn: "Id");
        }
    }
}
