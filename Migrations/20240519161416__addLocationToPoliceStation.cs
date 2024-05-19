using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace e_crime.Migrations
{
    /// <inheritdoc />
    public partial class _addLocationToPoliceStation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "County",
                table: "PoliceStations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "PoliceStations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "County",
                table: "PoliceStations");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "PoliceStations");
        }
    }
}
