using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ratatoskr.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PositionNumberInvoiceAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PositionNumber",
                table: "InvoiceItems",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PositionNumber",
                table: "InvoiceItems");
        }
    }
}
