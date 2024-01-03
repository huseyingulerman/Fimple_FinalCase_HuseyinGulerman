using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Fimple_FinalCase_HuseyinGulerman.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "5b64a78a-4442-4e92-a019-4f7bfb29ac52", null, "admin", "ADMIN" },
                    { "6b64a78a-4442-4e92-a019-4f7bfb29ac52", null, "user", "USER" },
                    { "7b64a78a-4442-4e92-a019-4f7bfb29ac52", null, "auditor", "AUDITOR" }
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00c92277-f154-49d4-9765-9be7357a04bd",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "DateOfBirth", "SecurityStamp" },
                values: new object[] { "da36647d-5dd4-488b-a40a-d6f2b1ee0d72", new DateTime(2024, 1, 3, 11, 21, 47, 458, DateTimeKind.Utc).AddTicks(1215), new DateTime(2024, 1, 3, 11, 21, 47, 458, DateTimeKind.Utc).AddTicks(1334), "5988ccfb-818a-44d5-b189-f730cadba8c0" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5b64a78a-4442-4e92-a019-4f7bfb29ac52");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6b64a78a-4442-4e92-a019-4f7bfb29ac52");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7b64a78a-4442-4e92-a019-4f7bfb29ac52");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00c92277-f154-49d4-9765-9be7357a04bd",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "DateOfBirth", "SecurityStamp" },
                values: new object[] { "508b6b5a-0b04-4229-adee-b895807dfd6e", new DateTime(2024, 1, 2, 22, 29, 49, 495, DateTimeKind.Utc).AddTicks(8727), new DateTime(2024, 1, 2, 22, 29, 49, 495, DateTimeKind.Utc).AddTicks(8769), "417fc192-3924-4fa2-adfb-d3274bbaf218" });
        }
    }
}
