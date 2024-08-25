using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace VillaApp.Infrastructure.VillaApp.Infrastructure
{
    /// <inheritdoc />
    public partial class AddTbl_VillaNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tbl_VillaNumber",
                columns: table => new
                {
                    Villa_Number = table.Column<int>(type: "integer", nullable: false),
                    VillaId = table.Column<int>(type: "integer", nullable: false),
                    SpecialDetails = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tbl_VillaNumber", x => x.Villa_Number);
                    table.ForeignKey(
                        name: "FK_Tbl_VillaNumber_Tbl_Villa_VillaId",
                        column: x => x.VillaId,
                        principalTable: "Tbl_Villa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Tbl_VillaNumber",
                columns: new[] { "Villa_Number", "SpecialDetails", "VillaId" },
                values: new object[,]
                {
                    { 101, null, 1 },
                    { 102, null, 1 },
                    { 103, null, 1 },
                    { 201, null, 2 },
                    { 202, null, 2 },
                    { 203, null, 2 },
                    { 301, null, 3 },
                    { 302, null, 3 },
                    { 303, null, 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tbl_VillaNumber_VillaId",
                table: "Tbl_VillaNumber",
                column: "VillaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tbl_VillaNumber");
        }
    }
}
