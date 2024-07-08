using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Projekt_server.Migrations
{
    /// <inheritdoc />
    public partial class AddViews : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            
            migrationBuilder.Sql(@"
            CREATE VIEW AppFilterView AS
            SELECT Id, Name
            FROM Apps
        GO");


            migrationBuilder.Sql(@"
            CREATE VIEW ServerFilterView AS
            SELECT Id, Name
            FROM Servers
        GO");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW IF EXISTS AppFilterView");
            migrationBuilder.Sql("DROP VIEW IF EXISTS ServerFilterView");
        }
    }
}
