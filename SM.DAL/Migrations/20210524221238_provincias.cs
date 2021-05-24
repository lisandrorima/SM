using Microsoft.EntityFrameworkCore.Migrations;

namespace SM.DAL.Migrations
{
    public partial class provincias : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProvinciaID",
                table: "RealEstates",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Provincias",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Provincias", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RealEstates_ProvinciaID",
                table: "RealEstates",
                column: "ProvinciaID");

            migrationBuilder.CreateIndex(
                name: "IX_Provincias_Nombre",
                table: "Provincias",
                column: "Nombre",
                unique: true,
                filter: "[Nombre] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_RealEstates_Provincias_ProvinciaID",
                table: "RealEstates",
                column: "ProvinciaID",
                principalTable: "Provincias",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RealEstates_Provincias_ProvinciaID",
                table: "RealEstates");

            migrationBuilder.DropTable(
                name: "Provincias");

            migrationBuilder.DropIndex(
                name: "IX_RealEstates_ProvinciaID",
                table: "RealEstates");

            migrationBuilder.DropColumn(
                name: "ProvinciaID",
                table: "RealEstates");
        }
    }
}
