using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExploreBulgaria.Data.Migrations
{
    public partial class AddRelationVoteVisitor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Votes_AspNetUsers_AddedByUserId",
                table: "Votes");

            migrationBuilder.RenameColumn(
                name: "AddedByUserId",
                table: "Votes",
                newName: "AddedByVisitorId");

            migrationBuilder.RenameIndex(
                name: "IX_Votes_AddedByUserId",
                table: "Votes",
                newName: "IX_Votes_AddedByVisitorId");

            migrationBuilder.AlterColumn<byte>(
                name: "Value",
                table: "Votes",
                type: "tinyint",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AddForeignKey(
                name: "FK_Votes_Visitors_AddedByVisitorId",
                table: "Votes",
                column: "AddedByVisitorId",
                principalTable: "Visitors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Votes_Visitors_AddedByVisitorId",
                table: "Votes");

            migrationBuilder.RenameColumn(
                name: "AddedByVisitorId",
                table: "Votes",
                newName: "AddedByUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Votes_AddedByVisitorId",
                table: "Votes",
                newName: "IX_Votes_AddedByUserId");

            migrationBuilder.AlterColumn<double>(
                name: "Value",
                table: "Votes",
                type: "float",
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.AddForeignKey(
                name: "FK_Votes_AspNetUsers_AddedByUserId",
                table: "Votes",
                column: "AddedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
