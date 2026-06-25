using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineStoreApp.Migrations
{
    /// <inheritdoc />
    public partial class AddClientItemClient : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Clientid",
                table: "Items",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ItemClients",
                columns: table => new
                {
                    itemId = table.Column<int>(type: "int", nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemClients", x => new { x.itemId, x.ClientId });
                    table.ForeignKey(
                        name: "FK_ItemClients_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItemClients_Items_itemId",
                        column: x => x.itemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Items",
                keyColumn: "Id",
                keyValue: 5,
                column: "Clientid",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_Items_Clientid",
                table: "Items",
                column: "Clientid");

            migrationBuilder.CreateIndex(
                name: "IX_ItemClients_ClientId",
                table: "ItemClients",
                column: "ClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Clients_Clientid",
                table: "Items",
                column: "Clientid",
                principalTable: "Clients",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Clients_Clientid",
                table: "Items");

            migrationBuilder.DropTable(
                name: "ItemClients");

            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropIndex(
                name: "IX_Items_Clientid",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "Clientid",
                table: "Items");
        }
    }
}
