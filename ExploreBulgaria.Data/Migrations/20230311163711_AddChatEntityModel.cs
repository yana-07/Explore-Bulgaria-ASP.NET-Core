using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExploreBulgaria.Data.Migrations
{
    public partial class AddChatEntityModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Chats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FromVisitorId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ToVisitorId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Chats_Visitors_FromVisitorId",
                        column: x => x.FromVisitorId,
                        principalTable: "Visitors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Chats_Visitors_ToVisitorId",
                        column: x => x.ToVisitorId,
                        principalTable: "Visitors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Chats_FromVisitorId",
                table: "Chats",
                column: "FromVisitorId");

            migrationBuilder.CreateIndex(
                name: "IX_Chats_IsDeleted",
                table: "Chats",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Chats_ToVisitorId",
                table: "Chats",
                column: "ToVisitorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Chats");
        }
    }
}
