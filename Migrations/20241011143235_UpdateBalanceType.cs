using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Argus_BalanceByAddressAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBalanceType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Balance_Value",
                table: "BalanceAddress",
                newName: "Balance");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Balance",
                table: "BalanceAddress",
                newName: "Balance_Value");
        }
    }
}
