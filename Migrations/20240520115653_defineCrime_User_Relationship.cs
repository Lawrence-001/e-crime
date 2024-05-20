using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace e_crime.Migrations
{
    /// <inheritdoc />
    public partial class defineCrime_User_Relationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserCrimes",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CrimeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCrimes", x => new { x.UserId, x.CrimeId });
                    table.ForeignKey(
                        name: "FK_UserCrimes_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserCrimes_Crimes_CrimeId",
                        column: x => x.CrimeId,
                        principalTable: "Crimes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserCrimes_CrimeId",
                table: "UserCrimes",
                column: "CrimeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserCrimes");
        }
    }
}
