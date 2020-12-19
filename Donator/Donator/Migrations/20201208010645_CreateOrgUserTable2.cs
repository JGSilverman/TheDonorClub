using Microsoft.EntityFrameworkCore.Migrations;

namespace Donator.Migrations
{
    public partial class CreateOrgUserTable2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrgUser_AspNetUsers_UserId",
                table: "OrgUser");

            migrationBuilder.DropForeignKey(
                name: "FK_OrgUser_NonProfitOrgs_NonProfitOrgId",
                table: "OrgUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrgUser",
                table: "OrgUser");

            migrationBuilder.RenameTable(
                name: "OrgUser",
                newName: "OrgUsers");

            migrationBuilder.RenameIndex(
                name: "IX_OrgUser_UserId",
                table: "OrgUsers",
                newName: "IX_OrgUsers_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_OrgUser_NonProfitOrgId",
                table: "OrgUsers",
                newName: "IX_OrgUsers_NonProfitOrgId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrgUsers",
                table: "OrgUsers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrgUsers_AspNetUsers_UserId",
                table: "OrgUsers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrgUsers_NonProfitOrgs_NonProfitOrgId",
                table: "OrgUsers",
                column: "NonProfitOrgId",
                principalTable: "NonProfitOrgs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrgUsers_AspNetUsers_UserId",
                table: "OrgUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_OrgUsers_NonProfitOrgs_NonProfitOrgId",
                table: "OrgUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrgUsers",
                table: "OrgUsers");

            migrationBuilder.RenameTable(
                name: "OrgUsers",
                newName: "OrgUser");

            migrationBuilder.RenameIndex(
                name: "IX_OrgUsers_UserId",
                table: "OrgUser",
                newName: "IX_OrgUser_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_OrgUsers_NonProfitOrgId",
                table: "OrgUser",
                newName: "IX_OrgUser_NonProfitOrgId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrgUser",
                table: "OrgUser",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrgUser_AspNetUsers_UserId",
                table: "OrgUser",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrgUser_NonProfitOrgs_NonProfitOrgId",
                table: "OrgUser",
                column: "NonProfitOrgId",
                principalTable: "NonProfitOrgs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
