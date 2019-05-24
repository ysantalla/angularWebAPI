using Microsoft.EntityFrameworkCore.Migrations;

namespace server.Migrations
{
    public partial class v14 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CurrencyId",
                table: "Invoices",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_CurrencyId",
                table: "Invoices",
                column: "CurrencyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_Currency_CurrencyId",
                table: "Invoices",
                column: "CurrencyId",
                principalTable: "Currency",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_Currency_CurrencyId",
                table: "Invoices");

            migrationBuilder.DropIndex(
                name: "IX_Invoices_CurrencyId",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "CurrencyId",
                table: "Invoices");
        }
    }
}
