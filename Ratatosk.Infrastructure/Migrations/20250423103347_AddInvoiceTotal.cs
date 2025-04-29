using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ratatoskr.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddInvoiceTotal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "TotalGross",
                table: "Invoices",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalNet",
                table: "Invoices",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalVat",
                table: "Invoices",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalGross",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "TotalNet",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "TotalVat",
                table: "Invoices");
        }
    }
}
