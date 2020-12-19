using Microsoft.EntityFrameworkCore.Migrations;

namespace Donator.Migrations
{
    public partial class AddIsActiveToOrgUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "OrgUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "OrgUsers");
        }
    }
}
