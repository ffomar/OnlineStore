using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace OnlineStoreApp.Migrations
{
    /// <inheritdoc />
    public partial class DataMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Clients",
                columns: new[] { "id", "Name" },
                values: new object[,]
                {
                    { 1, "Zac" },
                    { 2, "Martin" }
                });

            migrationBuilder.InsertData(
                table: "ItemClients",
                columns: new[] { "ClientId", "itemId" },
                values: new object[,]
                {
                    { 2, 4 },
                    { 1, 5 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ItemClients",
                keyColumns: new[] { "ClientId", "itemId" },
                keyValues: new object[] { 2, 4 });

            migrationBuilder.DeleteData(
                table: "ItemClients",
                keyColumns: new[] { "ClientId", "itemId" },
                keyValues: new object[] { 1, 5 });

            migrationBuilder.DeleteData(
                table: "Clients",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Clients",
                keyColumn: "id",
                keyValue: 2);
        }
    }
}
