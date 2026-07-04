using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GYM.Migrations
{
    /// <inheritdoc />
    public partial class AgregarRolAMiembro : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Rol",
                table: "Miembros",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rol",
                table: "Miembros");
        }
    }
}
