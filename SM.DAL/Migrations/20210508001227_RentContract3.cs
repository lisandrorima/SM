using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SM.DAL.Migrations
{
    public partial class RentContract3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RentContracts",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OwnerID = table.Column<int>(type: "int", nullable: true),
                    TenantID = table.Column<int>(type: "int", nullable: true),
                    RealEstateID = table.Column<int>(type: "int", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RentContracts", x => x.ID);
                    table.ForeignKey(
                        name: "FK_RentContracts_RealEstates_RealEstateID",
                        column: x => x.RealEstateID,
                        principalTable: "RealEstates",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RentContracts_Users_OwnerID",
                        column: x => x.OwnerID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RentContracts_Users_TenantID",
                        column: x => x.TenantID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RentContracts_OwnerID",
                table: "RentContracts",
                column: "OwnerID");

            migrationBuilder.CreateIndex(
                name: "IX_RentContracts_RealEstateID",
                table: "RentContracts",
                column: "RealEstateID");

            migrationBuilder.CreateIndex(
                name: "IX_RentContracts_TenantID",
                table: "RentContracts",
                column: "TenantID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RentContracts");
        }
    }
}
