using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Argus_BalanceByAddressAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTransactionsColNameisOutput : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Output",
                table: "Transactions",
                newName: "isOutput");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "isOutput",
                table: "Transactions",
                newName: "Output");
        }
    }
}
