using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExploreBulgaria.Data.Migrations
{
    public partial class ChatTableAddSentOnColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "SentOn",
                table: "Chats",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SentOn",
                table: "Chats");
        }
    }
}
