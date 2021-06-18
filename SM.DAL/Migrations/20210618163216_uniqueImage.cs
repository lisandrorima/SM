using Microsoft.EntityFrameworkCore.Migrations;

namespace SM.DAL.Migrations
{
    public partial class uniqueImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ImgURL",
                table: "ImagesRealEstate",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ImagesRealEstate_ImgURL",
                table: "ImagesRealEstate",
                column: "ImgURL",
                unique: true,
                filter: "[ImgURL] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ImagesRealEstate_ImgURL",
                table: "ImagesRealEstate");

            migrationBuilder.AlterColumn<string>(
                name: "ImgURL",
                table: "ImagesRealEstate",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}
