using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fimple_FinalCase_HuseyinGulerman.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddAccounIdColone1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TransactionSuccessful",
                table: "Processes",
                newName: "PendingApproval");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00c92277-f154-49d4-9765-9be7357a04bd",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "DateOfBirth", "SecurityStamp" },
                values: new object[] { "30933c3c-4019-4733-b6a7-b1065ff23dba", new DateTime(2024, 1, 6, 7, 59, 29, 505, DateTimeKind.Utc).AddTicks(7225), new DateTime(2024, 1, 6, 7, 59, 29, 505, DateTimeKind.Utc).AddTicks(7378), "4bd1b13e-5c57-4a42-bdf8-585fb1d5b38b" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PendingApproval",
                table: "Processes",
                newName: "TransactionSuccessful");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00c92277-f154-49d4-9765-9be7357a04bd",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "DateOfBirth", "SecurityStamp" },
                values: new object[] { "b6786d77-03b6-4fd4-8f9a-79fb497cb1c7", new DateTime(2024, 1, 6, 7, 42, 14, 837, DateTimeKind.Utc).AddTicks(5631), new DateTime(2024, 1, 6, 7, 42, 14, 837, DateTimeKind.Utc).AddTicks(5740), "1d9f7db0-531f-4e46-91dc-64178c33baca" });
        }
    }
}
