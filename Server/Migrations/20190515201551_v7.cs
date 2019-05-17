using Microsoft.EntityFrameworkCore.Migrations;

namespace server.Migrations
{
    public partial class v7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Countries_AspNetUsers_CreatorId",
                table: "Countries");

            migrationBuilder.DropIndex(
                name: "IX_Countries_CreatorId",
                table: "Countries");

            migrationBuilder.AddColumn<long>(
                name: "CreatorUserId",
                table: "Countries",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Countries_CreatorUserId",
                table: "Countries",
                column: "CreatorUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Countries_AspNetUsers_CreatorUserId",
                table: "Countries",
                column: "CreatorUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Countries_AspNetUsers_CreatorUserId",
                table: "Countries");

            migrationBuilder.DropIndex(
                name: "IX_Countries_CreatorUserId",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "CreatorUserId",
                table: "Countries");

            migrationBuilder.CreateIndex(
                name: "IX_Countries_CreatorId",
                table: "Countries",
                column: "CreatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Countries_AspNetUsers_CreatorId",
                table: "Countries",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
