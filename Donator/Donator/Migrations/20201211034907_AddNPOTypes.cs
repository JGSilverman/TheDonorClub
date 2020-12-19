using Microsoft.EntityFrameworkCore.Migrations;

namespace Donator.Migrations
{
    public partial class AddNPOTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NPOTypeId",
                table: "NonProfitOrgs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "NPOTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TaxCodeIdentifier = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NPOTypes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NonProfitOrgs_NPOTypeId",
                table: "NonProfitOrgs",
                column: "NPOTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_NonProfitOrgs_NPOTypes_NPOTypeId",
                table: "NonProfitOrgs",
                column: "NPOTypeId",
                principalTable: "NPOTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NonProfitOrgs_NPOTypes_NPOTypeId",
                table: "NonProfitOrgs");

            migrationBuilder.DropTable(
                name: "NPOTypes");

            migrationBuilder.DropIndex(
                name: "IX_NonProfitOrgs_NPOTypeId",
                table: "NonProfitOrgs");

            migrationBuilder.DropColumn(
                name: "NPOTypeId",
                table: "NonProfitOrgs");
        }
    }
}
