using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SM.DAL.Migrations
{
    public partial class CuponDePago : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Hash",
                table: "RentContracts",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "CuponDePagos",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HashCuponPago = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FechaVencimiento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsPayed = table.Column<bool>(type: "bit", nullable: false),
                    rentContractID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CuponDePagos", x => x.ID);
                    table.UniqueConstraint("AK_CuponDePagos_HashCuponPago", x => x.HashCuponPago);
                    table.ForeignKey(
                        name: "FK_CuponDePagos_RentContracts_rentContractID",
                        column: x => x.rentContractID,
                        principalTable: "RentContracts",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RentContracts_Hash",
                table: "RentContracts",
                column: "Hash",
                unique: true,
                filter: "[Hash] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CuponDePagos_HashCuponPago",
                table: "CuponDePagos",
                column: "HashCuponPago",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CuponDePagos_rentContractID",
                table: "CuponDePagos",
                column: "rentContractID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CuponDePagos");

            migrationBuilder.DropIndex(
                name: "IX_RentContracts_Hash",
                table: "RentContracts");

            migrationBuilder.AlterColumn<string>(
                name: "Hash",
                table: "RentContracts",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}
