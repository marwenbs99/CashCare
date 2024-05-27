using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CashCare.Migrations
{
    /// <inheritdoc />
    public partial class AddDescriptionToIncome : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DataOfRecive",
                table: "Incomes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataOfRecive",
                table: "Incomes");
        }
    }
}
