using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fimple_FinalCase_HuseyinGulerman.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddProcessTableColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsItAutomaticPayment",
                table: "Processes",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00c92277-f154-49d4-9765-9be7357a04bd",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "DateOfBirth", "SecurityStamp" },
                values: new object[] { "0dfa63d8-0c69-42ec-a3d8-122e1b1b0961", new DateTime(2024, 1, 6, 17, 31, 22, 811, DateTimeKind.Utc).AddTicks(1823), new DateTime(2024, 1, 6, 17, 31, 22, 811, DateTimeKind.Utc).AddTicks(1952), "710bcb8f-ad14-43ba-96b8-91773ce8598b" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsItAutomaticPayment",
                table: "Processes");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00c92277-f154-49d4-9765-9be7357a04bd",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "DateOfBirth", "SecurityStamp" },
                values: new object[] { "b94ba137-590b-4e8c-83aa-5731780d18e9", new DateTime(2024, 1, 6, 12, 12, 6, 404, DateTimeKind.Utc).AddTicks(1735), new DateTime(2024, 1, 6, 12, 12, 6, 404, DateTimeKind.Utc).AddTicks(1819), "5d941590-920c-48e2-a212-4f0c5581d551" });
        }
    }
}
