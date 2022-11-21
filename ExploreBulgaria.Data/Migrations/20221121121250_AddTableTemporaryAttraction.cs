using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExploreBulgaria.Data.Migrations
{
    public partial class AddTableTemporaryAttraction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TemporaryAttractions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", maxLength: 10000, nullable: false),
                    CategoryId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false),
                    Region = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    CreatedByVisitorId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    ImageGuids = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemporaryAttractions", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TemporaryAttractions_IsDeleted",
                table: "TemporaryAttractions",
                column: "IsDeleted");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TemporaryAttractions");
        }
    }
}
