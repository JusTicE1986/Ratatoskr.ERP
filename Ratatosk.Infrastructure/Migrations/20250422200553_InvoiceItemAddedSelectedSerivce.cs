using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ratatoskr.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InvoiceItemAddedSelectedSerivce : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ServiceId",
                table: "InvoiceItems",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceItems_ServiceId",
                table: "InvoiceItems",
                column: "ServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceItems_Services_ServiceId",
                table: "InvoiceItems",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceItems_Services_ServiceId",
                table: "InvoiceItems");

            migrationBuilder.DropIndex(
                name: "IX_InvoiceItems_ServiceId",
                table: "InvoiceItems");

            migrationBuilder.DropColumn(
                name: "ServiceId",
                table: "InvoiceItems");
        }
    }
}
