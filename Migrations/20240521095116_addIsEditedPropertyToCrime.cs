using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace e_crime.Migrations
{
    /// <inheritdoc />
    public partial class addIsEditedPropertyToCrime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsEdited",
                table: "Crimes",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsEdited",
                table: "Crimes");
        }
    }
}
