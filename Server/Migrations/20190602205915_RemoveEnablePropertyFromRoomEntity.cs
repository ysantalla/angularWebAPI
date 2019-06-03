using Microsoft.EntityFrameworkCore.Migrations;

namespace server.Migrations
{
    public partial class RemoveEnablePropertyFromRoomEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Enable",
                table: "Rooms");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Enable",
                table: "Rooms",
                nullable: false,
                defaultValue: false);
        }
    }
}
