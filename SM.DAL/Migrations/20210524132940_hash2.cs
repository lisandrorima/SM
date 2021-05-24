using Microsoft.EntityFrameworkCore.Migrations;

namespace SM.DAL.Migrations
{
    public partial class hash2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Hash",
                table: "RentContracts",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Hash",
                table: "RentContracts");
        }
    }
}
