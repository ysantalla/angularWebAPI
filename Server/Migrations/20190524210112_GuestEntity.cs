using Microsoft.EntityFrameworkCore.Migrations;

namespace server.Migrations
{
    public partial class GuestEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Guets_Citizenships_CitizenshipId",
                table: "Guets");

            migrationBuilder.DropForeignKey(
                name: "FK_Guets_Countries_CountryId",
                table: "Guets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Guets",
                table: "Guets");

            migrationBuilder.RenameTable(
                name: "Guets",
                newName: "Guest");

            migrationBuilder.RenameColumn(
                name: "CountryId",
                table: "Guest",
                newName: "CountryID");

            migrationBuilder.RenameColumn(
                name: "CitizenshipId",
                table: "Guest",
                newName: "CitizenshipID");

            migrationBuilder.RenameIndex(
                name: "IX_Guets_CountryId",
                table: "Guest",
                newName: "IX_Guest_CountryID");

            migrationBuilder.RenameIndex(
                name: "IX_Guets_CitizenshipId",
                table: "Guest",
                newName: "IX_Guest_CitizenshipID");

            migrationBuilder.AlterColumn<long>(
                name: "CountryID",
                table: "Guest",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "CitizenshipID",
                table: "Guest",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Guest",
                table: "Guest",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Guest_Citizenships_CitizenshipID",
                table: "Guest",
                column: "CitizenshipID",
                principalTable: "Citizenships",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Guest_Countries_CountryID",
                table: "Guest",
                column: "CountryID",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Guest_Citizenships_CitizenshipID",
                table: "Guest");

            migrationBuilder.DropForeignKey(
                name: "FK_Guest_Countries_CountryID",
                table: "Guest");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Guest",
                table: "Guest");

            migrationBuilder.RenameTable(
                name: "Guest",
                newName: "Guets");

            migrationBuilder.RenameColumn(
                name: "CountryID",
                table: "Guets",
                newName: "CountryId");

            migrationBuilder.RenameColumn(
                name: "CitizenshipID",
                table: "Guets",
                newName: "CitizenshipId");

            migrationBuilder.RenameIndex(
                name: "IX_Guest_CountryID",
                table: "Guets",
                newName: "IX_Guets_CountryId");

            migrationBuilder.RenameIndex(
                name: "IX_Guest_CitizenshipID",
                table: "Guets",
                newName: "IX_Guets_CitizenshipId");

            migrationBuilder.AlterColumn<long>(
                name: "CountryId",
                table: "Guets",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<long>(
                name: "CitizenshipId",
                table: "Guets",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Guets",
                table: "Guets",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Guets_Citizenships_CitizenshipId",
                table: "Guets",
                column: "CitizenshipId",
                principalTable: "Citizenships",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Guets_Countries_CountryId",
                table: "Guets",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
