using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExploreBulgaria.Data.Migrations
{
    public partial class AddVisitorFavoriteVisitorVisitedAndVisitorWantToVisitAttraction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VisitorFavoriteAttraction",
                columns: table => new
                {
                    VisitorId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AttractionId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VisitorFavoriteAttraction", x => new { x.VisitorId, x.AttractionId });
                    table.ForeignKey(
                        name: "FK_VisitorFavoriteAttraction_Attractions_AttractionId",
                        column: x => x.AttractionId,
                        principalTable: "Attractions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VisitorFavoriteAttraction_Visitors_VisitorId",
                        column: x => x.VisitorId,
                        principalTable: "Visitors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VisitorVisitedAttraction",
                columns: table => new
                {
                    VisitorId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AttractionId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VisitorVisitedAttraction", x => new { x.VisitorId, x.AttractionId });
                    table.ForeignKey(
                        name: "FK_VisitorVisitedAttraction_Attractions_AttractionId",
                        column: x => x.AttractionId,
                        principalTable: "Attractions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VisitorVisitedAttraction_Visitors_VisitorId",
                        column: x => x.VisitorId,
                        principalTable: "Visitors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VisitorWantToVisitAttraction",
                columns: table => new
                {
                    VisitorId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AttractionId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VisitorWantToVisitAttraction", x => new { x.VisitorId, x.AttractionId });
                    table.ForeignKey(
                        name: "FK_VisitorWantToVisitAttraction_Attractions_AttractionId",
                        column: x => x.AttractionId,
                        principalTable: "Attractions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VisitorWantToVisitAttraction_Visitors_VisitorId",
                        column: x => x.VisitorId,
                        principalTable: "Visitors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VisitorFavoriteAttraction_AttractionId",
                table: "VisitorFavoriteAttraction",
                column: "AttractionId");

            migrationBuilder.CreateIndex(
                name: "IX_VisitorVisitedAttraction_AttractionId",
                table: "VisitorVisitedAttraction",
                column: "AttractionId");

            migrationBuilder.CreateIndex(
                name: "IX_VisitorWantToVisitAttraction_AttractionId",
                table: "VisitorWantToVisitAttraction",
                column: "AttractionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VisitorFavoriteAttraction");

            migrationBuilder.DropTable(
                name: "VisitorVisitedAttraction");

            migrationBuilder.DropTable(
                name: "VisitorWantToVisitAttraction");
        }
    }
}
