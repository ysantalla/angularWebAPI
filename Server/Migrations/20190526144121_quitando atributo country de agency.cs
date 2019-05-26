using Microsoft.EntityFrameworkCore.Migrations;

namespace server.Migrations
{
    public partial class quitandoatributocountrydeagency : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Agencies_Countries_CountryID",
                table: "Agencies");

            migrationBuilder.DropIndex(
                name: "IX_Agencies_CountryID",
                table: "Agencies");

            migrationBuilder.DropColumn(
                name: "CountryID",
                table: "Agencies");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CountryID",
                table: "Agencies",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Agencies_CountryID",
                table: "Agencies",
                column: "CountryID");

            migrationBuilder.AddForeignKey(
                name: "FK_Agencies_Countries_CountryID",
                table: "Agencies",
                column: "CountryID",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
