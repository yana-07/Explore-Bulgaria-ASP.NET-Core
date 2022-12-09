using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExploreBulgaria.Data.Migrations
{
    public partial class AddUserDefinedFunction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @"CREATE FUNCTION ufn_GetGeographicDistance (@source geography, @target geography) RETURNS FLOAT
                    AS
                    BEGIN
                     DECLARE @distance FLOAT
                     SET @distance = @source.STDistance(@target)
                     RETURN @distance
                    END"
                );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP FUNCTION ufn_GetGeographicDistance");
        }
    }
}
