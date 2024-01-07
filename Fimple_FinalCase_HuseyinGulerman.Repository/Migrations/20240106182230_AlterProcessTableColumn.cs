using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fimple_FinalCase_HuseyinGulerman.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AlterProcessTableColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsItAutomaticPayment",
                table: "Processes");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00c92277-f154-49d4-9765-9be7357a04bd",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "DateOfBirth", "SecurityStamp" },
                values: new object[] { "b420f282-0f4f-4693-bc15-c45b0cd93f8f", new DateTime(2024, 1, 6, 18, 22, 30, 615, DateTimeKind.Utc).AddTicks(5643), new DateTime(2024, 1, 6, 18, 22, 30, 615, DateTimeKind.Utc).AddTicks(5719), "f76e3d09-98d8-41fc-aa45-03dca252e1f0" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
    }
}
