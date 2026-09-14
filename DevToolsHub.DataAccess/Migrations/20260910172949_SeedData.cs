using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DevToolsHub.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Plans",
                columns: new[] { "Id", "DurationInDays", "IsActive", "Name", "Price" },
                values: new object[,]
                {
                    { 1, 30, true, "Free", 0m },
                    { 2, 30, true, "Pro", 9.99m },
                    { 3, 30, true, "Team", 29.99m }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "System administrator", "Admin" },
                    { 2, "Normal application user", "User" }
                });

            migrationBuilder.InsertData(
                table: "Tools",
                columns: new[] { "Id", "Category", "CreatedAt", "Description", "IsActive", "Name" },
                values: new object[,]
                {
                    { 1, "JSON", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Format and validate JSON data", true, "JSON Formatter" },
                    { 2, "Authentication", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Decode JWT token payload", true, "JWT Decoder" },
                    { 3, "Encoding", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Encode and decode Base64 text", true, "Base64 Encoder Decoder" },
                    { 4, "Generators", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Generate UUID and GUID values", true, "UUID Generator" },
                    { 5, "Encoding", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Encode and decode URLs", true, "URL Encoder Decoder" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Plans",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Plans",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Plans",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Tools",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Tools",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Tools",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Tools",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Tools",
                keyColumn: "Id",
                keyValue: 5);
        }
    }
}
