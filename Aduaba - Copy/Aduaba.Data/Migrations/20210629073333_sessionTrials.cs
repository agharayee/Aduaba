using Microsoft.EntityFrameworkCore.Migrations;

namespace Aduaba.Data.Migrations
{
    public partial class sessionTrials : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CartWithSessionId",
                table: "Products",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CartWithSessions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    ShoppingCartId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartWithSessions", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_CartWithSessionId",
                table: "Products",
                column: "CartWithSessionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_CartWithSessions_CartWithSessionId",
                table: "Products",
                column: "CartWithSessionId",
                principalTable: "CartWithSessions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_CartWithSessions_CartWithSessionId",
                table: "Products");

            migrationBuilder.DropTable(
                name: "CartWithSessions");

            migrationBuilder.DropIndex(
                name: "IX_Products_CartWithSessionId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CartWithSessionId",
                table: "Products");
        }
    }
}
