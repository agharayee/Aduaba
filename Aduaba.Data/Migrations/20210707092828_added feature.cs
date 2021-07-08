using Microsoft.EntityFrameworkCore.Migrations;

namespace Aduaba.Data.Migrations
{
    public partial class addedfeature : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsBestSelling",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsFeaturedProduct",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsBestSelling",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "IsFeaturedProduct",
                table: "Products");
        }
    }
}
