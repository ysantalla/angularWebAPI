using Microsoft.EntityFrameworkCore.Migrations;

namespace server.Migrations
{
    public partial class v10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Citizenships_AspNetUsers_CreatorUserId",
                table: "Citizenships");

            migrationBuilder.DropForeignKey(
                name: "FK_Countries_AspNetUsers_CreatorUserId",
                table: "Countries");

            migrationBuilder.DropForeignKey(
                name: "FK_Currency_AspNetUsers_CreatorUserId",
                table: "Currency");

            migrationBuilder.DropForeignKey(
                name: "FK_Guets_AspNetUsers_CreatorUserId",
                table: "Guets");

            migrationBuilder.DropIndex(
                name: "IX_Guets_CreatorUserId",
                table: "Guets");

            migrationBuilder.DropIndex(
                name: "IX_Currency_CreatorUserId",
                table: "Currency");

            migrationBuilder.DropIndex(
                name: "IX_Countries_CreatorUserId",
                table: "Countries");

            migrationBuilder.DropIndex(
                name: "IX_Citizenships_CreatorUserId",
                table: "Citizenships");

            migrationBuilder.DropColumn(
                name: "CreatorUserId",
                table: "Guets");

            migrationBuilder.DropColumn(
                name: "CreatorUserId",
                table: "Currency");

            migrationBuilder.DropColumn(
                name: "CreatorUserId",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "CuntryId",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "CreatorUserId",
                table: "Citizenships");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CreatorUserId",
                table: "Guets",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CreatorUserId",
                table: "Currency",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CreatorUserId",
                table: "Countries",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CuntryId",
                table: "Countries",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "CreatorUserId",
                table: "Citizenships",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Guets_CreatorUserId",
                table: "Guets",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Currency_CreatorUserId",
                table: "Currency",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Countries_CreatorUserId",
                table: "Countries",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Citizenships_CreatorUserId",
                table: "Citizenships",
                column: "CreatorUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Citizenships_AspNetUsers_CreatorUserId",
                table: "Citizenships",
                column: "CreatorUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Countries_AspNetUsers_CreatorUserId",
                table: "Countries",
                column: "CreatorUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Currency_AspNetUsers_CreatorUserId",
                table: "Currency",
                column: "CreatorUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Guets_AspNetUsers_CreatorUserId",
                table: "Guets",
                column: "CreatorUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
