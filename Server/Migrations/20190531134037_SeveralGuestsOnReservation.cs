using Microsoft.EntityFrameworkCore.Migrations;

namespace server.Migrations
{
    public partial class SeveralGuestsOnReservation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Guests_GuestID",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_GuestID",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "GuestID",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "PackageID",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "Birthday",
                table: "Guests");

            migrationBuilder.AlterColumn<double>(
                name: "Number",
                table: "Invoices",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ReservationId",
                table: "Guests",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Guests_ReservationId",
                table: "Guests",
                column: "ReservationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Guests_Reservations_ReservationId",
                table: "Guests",
                column: "ReservationId",
                principalTable: "Reservations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Guests_Reservations_ReservationId",
                table: "Guests");

            migrationBuilder.DropIndex(
                name: "IX_Guests_ReservationId",
                table: "Guests");

            migrationBuilder.DropColumn(
                name: "ReservationId",
                table: "Guests");

            migrationBuilder.AddColumn<long>(
                name: "GuestID",
                table: "Reservations",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "PackageID",
                table: "Reservations",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AlterColumn<string>(
                name: "Number",
                table: "Invoices",
                nullable: true,
                oldClrType: typeof(double));

            migrationBuilder.AddColumn<string>(
                name: "Birthday",
                table: "Guests",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_GuestID",
                table: "Reservations",
                column: "GuestID");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Guests_GuestID",
                table: "Reservations",
                column: "GuestID",
                principalTable: "Guests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
