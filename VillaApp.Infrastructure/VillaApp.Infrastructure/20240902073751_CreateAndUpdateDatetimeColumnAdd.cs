using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VillaApp.Infrastructure.VillaApp.Infrastructure
{
    /// <inheritdoc />
    public partial class CreateAndUpdateDatetimeColumnAdd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Tbl_Villa",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "Tbl_Villa",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Tbl_Villa",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Tbl_Villa",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Tbl_Villa",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Tbl_Villa");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "Tbl_Villa");
        }
    }
}
