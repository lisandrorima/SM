using Microsoft.EntityFrameworkCore.Migrations;

namespace SM.DAL.Migrations
{
    public partial class validacionContratoYcupones : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Isvalid",
                table: "RentContracts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Isvalid",
                table: "CuponDePagos",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Isvalid",
                table: "RentContracts");

            migrationBuilder.DropColumn(
                name: "Isvalid",
                table: "CuponDePagos");
        }
    }
}
