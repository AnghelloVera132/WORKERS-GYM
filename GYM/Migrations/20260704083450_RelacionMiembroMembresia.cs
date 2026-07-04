using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GYM.Migrations
{
    /// <inheritdoc />
    public partial class RelacionMiembroMembresia : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Membresias_MiembroId",
                table: "Membresias",
                column: "MiembroId");

            migrationBuilder.AddForeignKey(
                name: "FK_Membresias_Miembros_MiembroId",
                table: "Membresias",
                column: "MiembroId",
                principalTable: "Miembros",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Membresias_Miembros_MiembroId",
                table: "Membresias");

            migrationBuilder.DropIndex(
                name: "IX_Membresias_MiembroId",
                table: "Membresias");
        }
    }
}
