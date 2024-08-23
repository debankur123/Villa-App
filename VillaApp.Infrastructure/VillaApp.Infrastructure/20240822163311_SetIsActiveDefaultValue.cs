using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VillaApp.Infrastructure.VillaApp.Infrastructure
{
    /// <inheritdoc />
    public partial class SetIsActiveDefaultValue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Tbl_Villa",
                keyColumn: "Id",
                keyValue: 1,
                column: "IsActive",
                value: true);

            migrationBuilder.UpdateData(
                table: "Tbl_Villa",
                keyColumn: "Id",
                keyValue: 2,
                column: "IsActive",
                value: true);

            migrationBuilder.UpdateData(
                table: "Tbl_Villa",
                keyColumn: "Id",
                keyValue: 3,
                column: "IsActive",
                value: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Tbl_Villa",
                keyColumn: "Id",
                keyValue: 1,
                column: "IsActive",
                value: false);

            migrationBuilder.UpdateData(
                table: "Tbl_Villa",
                keyColumn: "Id",
                keyValue: 2,
                column: "IsActive",
                value: false);

            migrationBuilder.UpdateData(
                table: "Tbl_Villa",
                keyColumn: "Id",
                keyValue: 3,
                column: "IsActive",
                value: false);
        }
    }
}
