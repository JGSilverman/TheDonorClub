using Microsoft.EntityFrameworkCore.Migrations;

namespace Donator.Migrations
{
    public partial class AddIsAdminOfOrgPropToUserOrgTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAdminOfOrg",
                table: "OrgUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAdminOfOrg",
                table: "OrgUsers");
        }
    }
}
