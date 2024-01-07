using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fimple_FinalCase_HuseyinGulerman.Repository.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00c92277-f154-49d4-9765-9be7357a04bd",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "DateOfBirth", "SecurityStamp" },
                values: new object[] { "10ea3b48-2aa1-4883-8a3c-f70d9fa93665", new DateTime(2024, 1, 7, 0, 28, 42, 559, DateTimeKind.Utc).AddTicks(5044), new DateTime(2024, 1, 7, 0, 28, 42, 559, DateTimeKind.Utc).AddTicks(5156), "892145e8-665c-42c5-8beb-ca3e785f5e06" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00c92277-f154-49d4-9765-9be7357a04bd",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "DateOfBirth", "SecurityStamp" },
                values: new object[] { "55ab83f2-433d-49fd-a8ab-df82eb16d00f", new DateTime(2024, 1, 6, 19, 17, 10, 175, DateTimeKind.Utc).AddTicks(9176), new DateTime(2024, 1, 6, 19, 17, 10, 175, DateTimeKind.Utc).AddTicks(9280), "6d6dac2a-ce07-4a33-b4c7-158df00c5dbd" });
        }
    }
}
