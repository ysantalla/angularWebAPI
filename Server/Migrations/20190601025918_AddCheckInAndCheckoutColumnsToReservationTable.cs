using Microsoft.EntityFrameworkCore.Migrations;

namespace server.Migrations
{
    public partial class AddCheckInAndCheckoutColumnsToReservationTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CheckIn",
                table: "Reservations",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CheckOut",
                table: "Reservations",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CheckIn",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "CheckOut",
                table: "Reservations");
        }
    }
}
