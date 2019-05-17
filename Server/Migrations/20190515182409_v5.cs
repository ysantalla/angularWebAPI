using Microsoft.EntityFrameworkCore.Migrations;

namespace server.Migrations
{
    public partial class v5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_citizenship_AspNetUsers_CreatorUserId",
                table: "citizenship");

            migrationBuilder.DropForeignKey(
                name: "FK_country_AspNetUsers_CreatorUserId",
                table: "country");

            migrationBuilder.DropForeignKey(
                name: "FK_currency_AspNetUsers_CreatorUserId",
                table: "currency");

            migrationBuilder.DropForeignKey(
                name: "FK_guets_citizenship_CitizenshipId",
                table: "guets");

            migrationBuilder.DropForeignKey(
                name: "FK_guets_country_CountryId",
                table: "guets");

            migrationBuilder.DropForeignKey(
                name: "FK_guets_AspNetUsers_CreatorUserId",
                table: "guets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_guets",
                table: "guets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_currency",
                table: "currency");

            migrationBuilder.DropPrimaryKey(
                name: "PK_country",
                table: "country");

            migrationBuilder.DropIndex(
                name: "IX_country_CreatorUserId",
                table: "country");

            migrationBuilder.DropPrimaryKey(
                name: "PK_citizenship",
                table: "citizenship");

            migrationBuilder.DropColumn(
                name: "CreatorUserId",
                table: "country");

            migrationBuilder.RenameTable(
                name: "guets",
                newName: "Guets");

            migrationBuilder.RenameTable(
                name: "currency",
                newName: "Currency");

            migrationBuilder.RenameTable(
                name: "country",
                newName: "Countries");

            migrationBuilder.RenameTable(
                name: "citizenship",
                newName: "Citizenships");

            migrationBuilder.RenameIndex(
                name: "IX_guets_CreatorUserId",
                table: "Guets",
                newName: "IX_Guets_CreatorUserId");

            migrationBuilder.RenameIndex(
                name: "IX_guets_CountryId",
                table: "Guets",
                newName: "IX_Guets_CountryId");

            migrationBuilder.RenameIndex(
                name: "IX_guets_CitizenshipId",
                table: "Guets",
                newName: "IX_Guets_CitizenshipId");

            migrationBuilder.RenameIndex(
                name: "IX_currency_CreatorUserId",
                table: "Currency",
                newName: "IX_Currency_CreatorUserId");

            migrationBuilder.RenameIndex(
                name: "IX_citizenship_CreatorUserId",
                table: "Citizenships",
                newName: "IX_Citizenships_CreatorUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Guets",
                table: "Guets",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Currency",
                table: "Currency",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Countries",
                table: "Countries",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Citizenships",
                table: "Citizenships",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Countries_CreatorId",
                table: "Countries",
                column: "CreatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Citizenships_AspNetUsers_CreatorUserId",
                table: "Citizenships",
                column: "CreatorUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Countries_AspNetUsers_CreatorId",
                table: "Countries",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Currency_AspNetUsers_CreatorUserId",
                table: "Currency",
                column: "CreatorUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

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

            migrationBuilder.AddForeignKey(
                name: "FK_Guets_AspNetUsers_CreatorUserId",
                table: "Guets",
                column: "CreatorUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Citizenships_AspNetUsers_CreatorUserId",
                table: "Citizenships");

            migrationBuilder.DropForeignKey(
                name: "FK_Countries_AspNetUsers_CreatorId",
                table: "Countries");

            migrationBuilder.DropForeignKey(
                name: "FK_Currency_AspNetUsers_CreatorUserId",
                table: "Currency");

            migrationBuilder.DropForeignKey(
                name: "FK_Guets_Citizenships_CitizenshipId",
                table: "Guets");

            migrationBuilder.DropForeignKey(
                name: "FK_Guets_Countries_CountryId",
                table: "Guets");

            migrationBuilder.DropForeignKey(
                name: "FK_Guets_AspNetUsers_CreatorUserId",
                table: "Guets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Guets",
                table: "Guets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Currency",
                table: "Currency");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Countries",
                table: "Countries");

            migrationBuilder.DropIndex(
                name: "IX_Countries_CreatorId",
                table: "Countries");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Citizenships",
                table: "Citizenships");

            migrationBuilder.RenameTable(
                name: "Guets",
                newName: "guets");

            migrationBuilder.RenameTable(
                name: "Currency",
                newName: "currency");

            migrationBuilder.RenameTable(
                name: "Countries",
                newName: "country");

            migrationBuilder.RenameTable(
                name: "Citizenships",
                newName: "citizenship");

            migrationBuilder.RenameIndex(
                name: "IX_Guets_CreatorUserId",
                table: "guets",
                newName: "IX_guets_CreatorUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Guets_CountryId",
                table: "guets",
                newName: "IX_guets_CountryId");

            migrationBuilder.RenameIndex(
                name: "IX_Guets_CitizenshipId",
                table: "guets",
                newName: "IX_guets_CitizenshipId");

            migrationBuilder.RenameIndex(
                name: "IX_Currency_CreatorUserId",
                table: "currency",
                newName: "IX_currency_CreatorUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Citizenships_CreatorUserId",
                table: "citizenship",
                newName: "IX_citizenship_CreatorUserId");

            migrationBuilder.AddColumn<long>(
                name: "CreatorUserId",
                table: "country",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_guets",
                table: "guets",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_currency",
                table: "currency",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_country",
                table: "country",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_citizenship",
                table: "citizenship",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_country_CreatorUserId",
                table: "country",
                column: "CreatorUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_citizenship_AspNetUsers_CreatorUserId",
                table: "citizenship",
                column: "CreatorUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_country_AspNetUsers_CreatorUserId",
                table: "country",
                column: "CreatorUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_currency_AspNetUsers_CreatorUserId",
                table: "currency",
                column: "CreatorUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_guets_citizenship_CitizenshipId",
                table: "guets",
                column: "CitizenshipId",
                principalTable: "citizenship",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_guets_country_CountryId",
                table: "guets",
                column: "CountryId",
                principalTable: "country",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_guets_AspNetUsers_CreatorUserId",
                table: "guets",
                column: "CreatorUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
