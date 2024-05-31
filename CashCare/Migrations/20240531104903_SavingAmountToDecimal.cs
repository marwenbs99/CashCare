﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CashCare.Migrations
{
    /// <inheritdoc />
    public partial class SavingAmountToDecimal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "MonthlySavingAmount",
                table: "Saving",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "MonthlySavingAmount",
                table: "Saving",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");
        }
    }
}
