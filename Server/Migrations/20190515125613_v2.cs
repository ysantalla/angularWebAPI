using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace server.Migrations
{
    public partial class v2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "citizenship",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatorUserId = table.Column<long>(nullable: true),
                    CreatorId = table.Column<long>(nullable: false),
                    ModifierId = table.Column<long>(nullable: false),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    ModifyDate = table.Column<DateTime>(nullable: false),
                    HVersion = table.Column<int>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CitizenshipId = table.Column<long>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_citizenship", x => x.Id);
                    table.ForeignKey(
                        name: "FK_citizenship_AspNetUsers_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "country",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatorUserId = table.Column<long>(nullable: true),
                    CreatorId = table.Column<long>(nullable: false),
                    ModifierId = table.Column<long>(nullable: false),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    ModifyDate = table.Column<DateTime>(nullable: false),
                    HVersion = table.Column<int>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CuntryId = table.Column<long>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_country", x => x.Id);
                    table.ForeignKey(
                        name: "FK_country_AspNetUsers_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "currency",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatorUserId = table.Column<long>(nullable: true),
                    CreatorId = table.Column<long>(nullable: false),
                    ModifierId = table.Column<long>(nullable: false),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    ModifyDate = table.Column<DateTime>(nullable: false),
                    HVersion = table.Column<int>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CurrencyId = table.Column<long>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Simbol = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_currency", x => x.Id);
                    table.ForeignKey(
                        name: "FK_currency_AspNetUsers_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "guets",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatorUserId = table.Column<long>(nullable: true),
                    CreatorId = table.Column<long>(nullable: false),
                    ModifierId = table.Column<long>(nullable: false),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    ModifyDate = table.Column<DateTime>(nullable: false),
                    HVersion = table.Column<int>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    GuestId = table.Column<long>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    Identification = table.Column<string>(nullable: true),
                    Birthday = table.Column<string>(nullable: true),
                    CountryId = table.Column<long>(nullable: true),
                    CitizenshipId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_guets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_guets_citizenship_CitizenshipId",
                        column: x => x.CitizenshipId,
                        principalTable: "citizenship",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_guets_country_CountryId",
                        column: x => x.CountryId,
                        principalTable: "country",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_guets_AspNetUsers_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_citizenship_CreatorUserId",
                table: "citizenship",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_country_CreatorUserId",
                table: "country",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_currency_CreatorUserId",
                table: "currency",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_guets_CitizenshipId",
                table: "guets",
                column: "CitizenshipId");

            migrationBuilder.CreateIndex(
                name: "IX_guets_CountryId",
                table: "guets",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_guets_CreatorUserId",
                table: "guets",
                column: "CreatorUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "currency");

            migrationBuilder.DropTable(
                name: "guets");

            migrationBuilder.DropTable(
                name: "citizenship");

            migrationBuilder.DropTable(
                name: "country");
        }
    }
}
