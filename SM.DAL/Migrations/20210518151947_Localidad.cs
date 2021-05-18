using Microsoft.EntityFrameworkCore.Migrations;

namespace SM.DAL.Migrations
{
    public partial class Localidad : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Localidad",
                table: "RealEstates",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Localidad",
                table: "RealEstates");
        }
    }
}
