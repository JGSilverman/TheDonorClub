using Microsoft.EntityFrameworkCore.Migrations;

namespace Donator.Migrations
{
    public partial class addnpotaxid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TaxId",
                table: "NonProfitOrgs",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TaxId",
                table: "NonProfitOrgs");
        }
    }
}
