using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.UsuarioService.Migrations
{
    /// <inheritdoc />
    public partial class AddUsuarioFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Reserva",
                table: "Usuarios",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "Usuarios",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "Salario",
                table: "Usuarios",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Credenciais_Usuarios_UsuarioId1",
                table: "Credenciais");

            migrationBuilder.DropIndex(
                name: "IX_Credenciais_UsuarioId1",
                table: "Credenciais");

            migrationBuilder.DropColumn(
                name: "Reserva",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "Salario",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "UsuarioId1",
                table: "Credenciais");
        }
    }
}
