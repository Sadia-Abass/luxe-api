using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace luxe.Server.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddingSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "dbo",
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "DateCreated", "Description", "IsActive", "LastUpdated", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "033d781c-9def-4b3f-a8eb-b57e82488b09", "c72ecc40-2878-4fd4-b889-a6a08c42b01f", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Customer Service", "CUSTOMER SERVICE" },
                    { "051d3bd0-0657-4fcf-aaf1-cb70a9031e22", "58f1a09a-5db1-4a95-a159-1afdd6275ea8", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Customer", "CUSTOMER" },
                    { "1d840a30-6f70-47b4-b065-e2ab55dd440e", "15f65fb7-388f-4b55-81d9-8c6f9d7053f3", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Manager", "MANAGER" },
                    { "4ab9a85f-46cb-489b-8f16-88706486fb71", "8e63e268-2b81-44fe-b1b6-e4c427b6f4f6", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin", "ADMIN" },
                    { "bdd6f1e5-f61c-4627-9db5-249731433f67", "37287c99-a424-447c-b878-a56079b63e82", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Super Admin", "SUPER ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "Roles",
                keyColumn: "Id",
                keyValue: "033d781c-9def-4b3f-a8eb-b57e82488b09");

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "Roles",
                keyColumn: "Id",
                keyValue: "051d3bd0-0657-4fcf-aaf1-cb70a9031e22");

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "Roles",
                keyColumn: "Id",
                keyValue: "1d840a30-6f70-47b4-b065-e2ab55dd440e");

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "Roles",
                keyColumn: "Id",
                keyValue: "4ab9a85f-46cb-489b-8f16-88706486fb71");

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "Roles",
                keyColumn: "Id",
                keyValue: "bdd6f1e5-f61c-4627-9db5-249731433f67");
        }
    }
}
