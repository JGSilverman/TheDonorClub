using Microsoft.EntityFrameworkCore.Migrations;

namespace Donator.Migrations
{
    public partial class ChangePropName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NonProfitOrgs_NPOTypes_NPOTypeId",
                table: "NonProfitOrgs");

            migrationBuilder.RenameColumn(
                name: "NPOTypeId",
                table: "NonProfitOrgs",
                newName: "TypeId");

            migrationBuilder.RenameIndex(
                name: "IX_NonProfitOrgs_NPOTypeId",
                table: "NonProfitOrgs",
                newName: "IX_NonProfitOrgs_TypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_NonProfitOrgs_NPOTypes_TypeId",
                table: "NonProfitOrgs",
                column: "TypeId",
                principalTable: "NPOTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NonProfitOrgs_NPOTypes_TypeId",
                table: "NonProfitOrgs");

            migrationBuilder.RenameColumn(
                name: "TypeId",
                table: "NonProfitOrgs",
                newName: "NPOTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_NonProfitOrgs_TypeId",
                table: "NonProfitOrgs",
                newName: "IX_NonProfitOrgs_NPOTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_NonProfitOrgs_NPOTypes_NPOTypeId",
                table: "NonProfitOrgs",
                column: "NPOTypeId",
                principalTable: "NPOTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
