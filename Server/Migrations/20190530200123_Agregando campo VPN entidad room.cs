using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace server.Migrations
{
    public partial class AgregandocampoVPNentidadroom : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Packages_PackageID",
                table: "Reservations");

            migrationBuilder.DropTable(
                name: "Packages");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_PackageID",
                table: "Reservations");

            migrationBuilder.AddColumn<double>(
                name: "VPN",
                table: "Rooms",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VPN",
                table: "Rooms");

            migrationBuilder.CreateTable(
                name: "Packages",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<long>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    HVersion = table.Column<int>(nullable: false),
                    ModifierId = table.Column<long>(nullable: false),
                    ModifyDate = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Packages", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_PackageID",
                table: "Reservations",
                column: "PackageID");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Packages_PackageID",
                table: "Reservations",
                column: "PackageID",
                principalTable: "Packages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
