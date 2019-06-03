using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace server.Migrations
{
    public partial class RemoveBaseModelSuperClassFromGuestReservationTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreateDate",
                table: "GuestReservations");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "GuestReservations");

            migrationBuilder.DropColumn(
                name: "HVersion",
                table: "GuestReservations");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "GuestReservations");

            migrationBuilder.DropColumn(
                name: "ModifierId",
                table: "GuestReservations");

            migrationBuilder.DropColumn(
                name: "ModifyDate",
                table: "GuestReservations");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDate",
                table: "GuestReservations",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "CreatorId",
                table: "GuestReservations",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<int>(
                name: "HVersion",
                table: "GuestReservations",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<long>(
                name: "Id",
                table: "GuestReservations",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "ModifierId",
                table: "GuestReservations",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifyDate",
                table: "GuestReservations",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
