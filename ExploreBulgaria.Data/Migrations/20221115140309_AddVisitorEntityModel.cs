using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExploreBulgaria.Data.Migrations
{
    public partial class AddVisitorEntityModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attractions_AspNetUsers_CreatedByUserId",
                table: "Attractions");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_AspNetUsers_AddedByUserId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Images_AspNetUsers_AddedByUserId",
                table: "Images");

            migrationBuilder.DropForeignKey(
                name: "FK_Replies_AspNetUsers_AuthorId",
                table: "Replies");

            migrationBuilder.DropTable(
                name: "UserDislikedComment");

            migrationBuilder.DropTable(
                name: "UserLikedComment");

            migrationBuilder.RenameColumn(
                name: "AddedByUserId",
                table: "Images",
                newName: "AddedByVisitorId");

            migrationBuilder.RenameIndex(
                name: "IX_Images_AddedByUserId",
                table: "Images",
                newName: "IX_Images_AddedByVisitorId");

            migrationBuilder.RenameColumn(
                name: "AddedByUserId",
                table: "Comments",
                newName: "AddedByVisitorId");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_AddedByUserId",
                table: "Comments",
                newName: "IX_Comments_AddedByVisitorId");

            migrationBuilder.RenameColumn(
                name: "CreatedByUserId",
                table: "Attractions",
                newName: "CreatedByVisitorId");

            migrationBuilder.RenameIndex(
                name: "IX_Attractions_CreatedByUserId",
                table: "Attractions",
                newName: "IX_Attractions_CreatedByVisitorId");

            migrationBuilder.CreateTable(
                name: "Visitors",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Visitors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Visitors_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VisitorDislikedComment",
                columns: table => new
                {
                    VisitorId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CommentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VisitorDislikedComment", x => new { x.VisitorId, x.CommentId });
                    table.ForeignKey(
                        name: "FK_VisitorDislikedComment_Comments_CommentId",
                        column: x => x.CommentId,
                        principalTable: "Comments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VisitorDislikedComment_Visitors_VisitorId",
                        column: x => x.VisitorId,
                        principalTable: "Visitors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VisitorLikedComment",
                columns: table => new
                {
                    VisitorId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CommentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VisitorLikedComment", x => new { x.VisitorId, x.CommentId });
                    table.ForeignKey(
                        name: "FK_VisitorLikedComment_Comments_CommentId",
                        column: x => x.CommentId,
                        principalTable: "Comments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VisitorLikedComment_Visitors_VisitorId",
                        column: x => x.VisitorId,
                        principalTable: "Visitors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VisitorDislikedComment_CommentId",
                table: "VisitorDislikedComment",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_VisitorLikedComment_CommentId",
                table: "VisitorLikedComment",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_Visitors_IsDeleted",
                table: "Visitors",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Visitors_UserId",
                table: "Visitors",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attractions_Visitors_CreatedByVisitorId",
                table: "Attractions",
                column: "CreatedByVisitorId",
                principalTable: "Visitors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Visitors_AddedByVisitorId",
                table: "Comments",
                column: "AddedByVisitorId",
                principalTable: "Visitors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Visitors_AddedByVisitorId",
                table: "Images",
                column: "AddedByVisitorId",
                principalTable: "Visitors",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Replies_Visitors_AuthorId",
                table: "Replies",
                column: "AuthorId",
                principalTable: "Visitors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attractions_Visitors_CreatedByVisitorId",
                table: "Attractions");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Visitors_AddedByVisitorId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Images_Visitors_AddedByVisitorId",
                table: "Images");

            migrationBuilder.DropForeignKey(
                name: "FK_Replies_Visitors_AuthorId",
                table: "Replies");

            migrationBuilder.DropTable(
                name: "VisitorDislikedComment");

            migrationBuilder.DropTable(
                name: "VisitorLikedComment");

            migrationBuilder.DropTable(
                name: "Visitors");

            migrationBuilder.RenameColumn(
                name: "AddedByVisitorId",
                table: "Images",
                newName: "AddedByUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Images_AddedByVisitorId",
                table: "Images",
                newName: "IX_Images_AddedByUserId");

            migrationBuilder.RenameColumn(
                name: "AddedByVisitorId",
                table: "Comments",
                newName: "AddedByUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_AddedByVisitorId",
                table: "Comments",
                newName: "IX_Comments_AddedByUserId");

            migrationBuilder.RenameColumn(
                name: "CreatedByVisitorId",
                table: "Attractions",
                newName: "CreatedByUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Attractions_CreatedByVisitorId",
                table: "Attractions",
                newName: "IX_Attractions_CreatedByUserId");

            migrationBuilder.CreateTable(
                name: "UserDislikedComment",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CommentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDislikedComment", x => new { x.UserId, x.CommentId });
                    table.ForeignKey(
                        name: "FK_UserDislikedComment_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserDislikedComment_Comments_CommentId",
                        column: x => x.CommentId,
                        principalTable: "Comments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserLikedComment",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CommentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLikedComment", x => new { x.UserId, x.CommentId });
                    table.ForeignKey(
                        name: "FK_UserLikedComment_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserLikedComment_Comments_CommentId",
                        column: x => x.CommentId,
                        principalTable: "Comments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserDislikedComment_CommentId",
                table: "UserDislikedComment",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLikedComment_CommentId",
                table: "UserLikedComment",
                column: "CommentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attractions_AspNetUsers_CreatedByUserId",
                table: "Attractions",
                column: "CreatedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_AspNetUsers_AddedByUserId",
                table: "Comments",
                column: "AddedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Images_AspNetUsers_AddedByUserId",
                table: "Images",
                column: "AddedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Replies_AspNetUsers_AuthorId",
                table: "Replies",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
