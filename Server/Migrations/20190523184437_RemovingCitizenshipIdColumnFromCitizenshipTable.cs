using Microsoft.EntityFrameworkCore.Migrations;

namespace server.Migrations
{
    public partial class RemovingCitizenshipIdColumnFromCitizenshipTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CitizenshipId",
                table: "Citizenships");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CitizenshipId",
                table: "Citizenships",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
