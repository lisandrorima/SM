using Microsoft.EntityFrameworkCore.Migrations;

namespace SM.DAL.Migrations
{
    public partial class addTXhash : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MinedOnBlock",
                table: "CuponDePagos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TXHash",
                table: "CuponDePagos",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MinedOnBlock",
                table: "CuponDePagos");

            migrationBuilder.DropColumn(
                name: "TXHash",
                table: "CuponDePagos");
        }
    }
}
