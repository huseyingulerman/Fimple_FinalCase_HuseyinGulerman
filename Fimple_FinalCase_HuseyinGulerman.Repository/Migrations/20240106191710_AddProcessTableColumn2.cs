using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fimple_FinalCase_HuseyinGulerman.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddProcessTableColumn2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "PaymentDate",
                table: "Processes",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00c92277-f154-49d4-9765-9be7357a04bd",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "DateOfBirth", "SecurityStamp" },
                values: new object[] { "55ab83f2-433d-49fd-a8ab-df82eb16d00f", new DateTime(2024, 1, 6, 19, 17, 10, 175, DateTimeKind.Utc).AddTicks(9176), new DateTime(2024, 1, 6, 19, 17, 10, 175, DateTimeKind.Utc).AddTicks(9280), "6d6dac2a-ce07-4a33-b4c7-158df00c5dbd" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentDate",
                table: "Processes");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00c92277-f154-49d4-9765-9be7357a04bd",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "DateOfBirth", "SecurityStamp" },
                values: new object[] { "b420f282-0f4f-4693-bc15-c45b0cd93f8f", new DateTime(2024, 1, 6, 18, 22, 30, 615, DateTimeKind.Utc).AddTicks(5643), new DateTime(2024, 1, 6, 18, 22, 30, 615, DateTimeKind.Utc).AddTicks(5719), "f76e3d09-98d8-41fc-aa45-03dca252e1f0" });
        }
    }
}
