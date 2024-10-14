using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Argus_BalanceByAddressAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTransactionPK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Transactions",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "Slot",
                table: "BalanceAddress");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Transactions",
                newName: "TxHash");

            migrationBuilder.AddColumn<decimal>(
                name: "TxIndex",
                table: "Transactions",
                type: "numeric(20,0)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Transactions",
                table: "Transactions",
                columns: new[] { "TxHash", "TxIndex" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Transactions",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "TxIndex",
                table: "Transactions");

            migrationBuilder.RenameColumn(
                name: "TxHash",
                table: "Transactions",
                newName: "ID");

            migrationBuilder.AddColumn<decimal>(
                name: "Slot",
                table: "BalanceAddress",
                type: "numeric(20,0)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Transactions",
                table: "Transactions",
                column: "ID");
        }
    }
}
