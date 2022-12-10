using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mango.Services.OrderAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateOrderHeaderMissingValues : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CVV",
                table: "OrderHeaders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ExpiryMonthYear",
                table: "OrderHeaders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CVV",
                table: "OrderHeaders");

            migrationBuilder.DropColumn(
                name: "ExpiryMonthYear",
                table: "OrderHeaders");
        }
    }
}
