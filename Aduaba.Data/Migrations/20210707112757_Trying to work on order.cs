using Microsoft.EntityFrameworkCore.Migrations;

namespace Aduaba.Data.Migrations
{
    public partial class Tryingtoworkonorder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentStatus",
                table: "Orders");

            migrationBuilder.AddColumn<bool>(
                name: "PaymentStatus",
                table: "OrderStatus",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalAmountToPay",
                table: "Orders",
                type: "Money",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<decimal>(
                name: "CartItemTotal",
                table: "CartItems",
                type: "Money",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentStatus",
                table: "OrderStatus");

            migrationBuilder.DropColumn(
                name: "CartItemTotal",
                table: "CartItems");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalAmountToPay",
                table: "Orders",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "Money");

            migrationBuilder.AddColumn<bool>(
                name: "PaymentStatus",
                table: "Orders",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
