using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Donator.Migrations
{
    public partial class AddRequestForVolunteers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RequestForVolunteers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FromTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    ToTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    NPOId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    WaiverRequired = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedByUserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestForVolunteers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RequestForVolunteers_AspNetUsers_UpdatedByUserId",
                        column: x => x.UpdatedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RequestForVolunteers_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RequestForVolunteers_NonProfitOrgs_NPOId",
                        column: x => x.NPOId,
                        principalTable: "NonProfitOrgs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RequestForVolunteers_NPOId",
                table: "RequestForVolunteers",
                column: "NPOId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestForVolunteers_UpdatedByUserId",
                table: "RequestForVolunteers",
                column: "UpdatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestForVolunteers_UserId",
                table: "RequestForVolunteers",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RequestForVolunteers");
        }
    }
}
