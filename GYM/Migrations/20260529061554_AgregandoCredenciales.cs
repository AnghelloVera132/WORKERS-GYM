using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GYM.Migrations
{
    /// <inheritdoc />
    public partial class AgregandoCredenciales : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Contraseña",
                table: "Miembros",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Correo",
                table: "Miembros",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Contraseña",
                table: "Miembros");

            migrationBuilder.DropColumn(
                name: "Correo",
                table: "Miembros");
        }
    }
}
