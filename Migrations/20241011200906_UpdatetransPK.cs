using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Argus_BalanceByAddressAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdatetransPK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Transactions",
                table: "Transactions");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Transactions",
                table: "Transactions",
                columns: new[] { "TxHash", "TxIndex", "isOutput" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Transactions",
                table: "Transactions");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Transactions",
                table: "Transactions",
                columns: new[] { "TxHash", "TxIndex" });
        }
    }
}
