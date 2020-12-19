using Microsoft.EntityFrameworkCore.Migrations;

namespace Donator.Migrations
{
    public partial class CreateOrgUserTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CoverImgUrl",
                table: "NonProfitOrgs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "NonProfitOrgs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LogoUrl",
                table: "NonProfitOrgs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WebsiteUrl",
                table: "NonProfitOrgs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "OrgUser",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    NonProfitOrgId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrgUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrgUser_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrgUser_NonProfitOrgs_NonProfitOrgId",
                        column: x => x.NonProfitOrgId,
                        principalTable: "NonProfitOrgs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrgUser_NonProfitOrgId",
                table: "OrgUser",
                column: "NonProfitOrgId");

            migrationBuilder.CreateIndex(
                name: "IX_OrgUser_UserId",
                table: "OrgUser",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrgUser");

            migrationBuilder.DropColumn(
                name: "CoverImgUrl",
                table: "NonProfitOrgs");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "NonProfitOrgs");

            migrationBuilder.DropColumn(
                name: "LogoUrl",
                table: "NonProfitOrgs");

            migrationBuilder.DropColumn(
                name: "WebsiteUrl",
                table: "NonProfitOrgs");
        }
    }
}
