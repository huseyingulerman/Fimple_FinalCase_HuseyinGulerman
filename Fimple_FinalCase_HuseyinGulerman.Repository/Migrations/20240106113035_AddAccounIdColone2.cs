using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fimple_FinalCase_HuseyinGulerman.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddAccounIdColone2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PendingApproval",
                table: "Processes");

            migrationBuilder.AddColumn<int>(
                name: "ProcessStatus",
                table: "Processes",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00c92277-f154-49d4-9765-9be7357a04bd",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "DateOfBirth", "SecurityStamp" },
                values: new object[] { "08903570-b74a-435d-8a9a-43e78a4ba445", new DateTime(2024, 1, 6, 11, 30, 34, 822, DateTimeKind.Utc).AddTicks(7784), new DateTime(2024, 1, 6, 11, 30, 34, 822, DateTimeKind.Utc).AddTicks(7876), "66a9b307-7951-4a89-bad6-dc79bef0b565" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProcessStatus",
                table: "Processes");

            migrationBuilder.AddColumn<bool>(
                name: "PendingApproval",
                table: "Processes",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00c92277-f154-49d4-9765-9be7357a04bd",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "DateOfBirth", "SecurityStamp" },
                values: new object[] { "30933c3c-4019-4733-b6a7-b1065ff23dba", new DateTime(2024, 1, 6, 7, 59, 29, 505, DateTimeKind.Utc).AddTicks(7225), new DateTime(2024, 1, 6, 7, 59, 29, 505, DateTimeKind.Utc).AddTicks(7378), "4bd1b13e-5c57-4a42-bdf8-585fb1d5b38b" });
        }
    }
}
