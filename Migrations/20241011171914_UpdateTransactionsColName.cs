using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Argus_BalanceByAddressAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTransactionsColName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Transactions",
                newName: "Output");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Output",
                table: "Transactions",
                newName: "Type");
        }
    }
}
