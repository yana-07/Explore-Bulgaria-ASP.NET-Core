using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExploreBulgaria.Data.Migrations
{
    public partial class RemoveApplicationUserAttractionMappingTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AttractionUserAddedToFavorites");

            migrationBuilder.DropTable(
                name: "AttractionUserVisited");

            migrationBuilder.DropTable(
                name: "AttractionUserWantToVisit");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AttractionUserAddedToFavorites",
                columns: table => new
                {
                    AttractionId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttractionUserAddedToFavorites", x => new { x.AttractionId, x.UserId });
                    table.ForeignKey(
                        name: "FK_AttractionUserAddedToFavorites_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AttractionUserAddedToFavorites_Attractions_AttractionId",
                        column: x => x.AttractionId,
                        principalTable: "Attractions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AttractionUserVisited",
                columns: table => new
                {
                    AttractionId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttractionUserVisited", x => new { x.AttractionId, x.UserId });
                    table.ForeignKey(
                        name: "FK_AttractionUserVisited_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AttractionUserVisited_Attractions_AttractionId",
                        column: x => x.AttractionId,
                        principalTable: "Attractions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AttractionUserWantToVisit",
                columns: table => new
                {
                    AttractionId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttractionUserWantToVisit", x => new { x.AttractionId, x.UserId });
                    table.ForeignKey(
                        name: "FK_AttractionUserWantToVisit_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AttractionUserWantToVisit_Attractions_AttractionId",
                        column: x => x.AttractionId,
                        principalTable: "Attractions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AttractionUserAddedToFavorites_UserId",
                table: "AttractionUserAddedToFavorites",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AttractionUserVisited_UserId",
                table: "AttractionUserVisited",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AttractionUserWantToVisit_UserId",
                table: "AttractionUserWantToVisit",
                column: "UserId");
        }
    }
}
