using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExploreBulgaria.Data.Migrations
{
    public partial class SetRegionNotNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attractions_Regions_RegionId",
                table: "Attractions");

            migrationBuilder.AlterColumn<string>(
                name: "RegionId",
                table: "Attractions",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Attractions_Regions_RegionId",
                table: "Attractions",
                column: "RegionId",
                principalTable: "Regions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attractions_Regions_RegionId",
                table: "Attractions");

            migrationBuilder.AlterColumn<string>(
                name: "RegionId",
                table: "Attractions",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_Attractions_Regions_RegionId",
                table: "Attractions",
                column: "RegionId",
                principalTable: "Regions",
                principalColumn: "Id");
        }
    }
}
