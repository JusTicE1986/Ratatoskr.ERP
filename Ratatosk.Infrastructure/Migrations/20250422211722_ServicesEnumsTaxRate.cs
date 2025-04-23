using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ratatoskr.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ServicesEnumsTaxRate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TaxRate",
                table: "Services",
                newName: "TaxRateEnum");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TaxRateEnum",
                table: "Services",
                newName: "TaxRate");
        }
    }
}
