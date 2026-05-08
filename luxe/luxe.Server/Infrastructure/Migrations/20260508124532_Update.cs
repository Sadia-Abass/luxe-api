using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace luxe.Server.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "Roles",
                keyColumn: "Id",
                keyValue: "30fb5645-a2c4-4f35-965a-681d7cd6f50f");

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "Roles",
                keyColumn: "Id",
                keyValue: "41a3eda1-baf7-47b1-b38b-9b4171757b1c");

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "Roles",
                keyColumn: "Id",
                keyValue: "926dff05-c6b4-477c-ae5f-eaf194fa0a34");

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "Roles",
                keyColumn: "Id",
                keyValue: "b2ca850a-d75b-4742-821c-c7012ea7f44e");

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "Roles",
                keyColumn: "Id",
                keyValue: "daae7cec-db20-4ccd-8e23-e7ae0feb9249");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "dbo",
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "DateCreated", "Description", "IsActive", "LastUpdated", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "30fb5645-a2c4-4f35-965a-681d7cd6f50f", "6639c7d0-4f4b-4dad-9f98-f64616b42b9b", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Super Admin", "SUPER ADMIN" },
                    { "41a3eda1-baf7-47b1-b38b-9b4171757b1c", "5c64eec5-c295-4cdd-8b11-7eb43eae5fad", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin", "ADMIN" },
                    { "926dff05-c6b4-477c-ae5f-eaf194fa0a34", "fc6c8bfb-b2f2-461e-ab02-dba273d67580", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Manager", "MANAGER" },
                    { "b2ca850a-d75b-4742-821c-c7012ea7f44e", "ed48c4ad-3c61-4a92-8841-0ed51c60d71c", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Customer", "CUSTOMER" },
                    { "daae7cec-db20-4ccd-8e23-e7ae0feb9249", "04159f8a-6527-4bfd-827e-7f857db9aefc", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Customer Service", "CUSTOMER SERVICE" }
                });
        }
    }
}
