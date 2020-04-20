using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Wallace.Persistence.Migrations
{
    public partial class AllowNullFieldsInTransactions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Categories_CategoryId",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Payees_PayeeId",
                table: "Transactions");

            migrationBuilder.AlterColumn<Guid>(
                name: "PayeeId",
                table: "Transactions",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "CategoryId",
                table: "Transactions",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Categories_CategoryId",
                table: "Transactions",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Payees_PayeeId",
                table: "Transactions",
                column: "PayeeId",
                principalTable: "Payees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Categories_CategoryId",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Payees_PayeeId",
                table: "Transactions");

            migrationBuilder.AlterColumn<Guid>(
                name: "PayeeId",
                table: "Transactions",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CategoryId",
                table: "Transactions",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Categories_CategoryId",
                table: "Transactions",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Payees_PayeeId",
                table: "Transactions",
                column: "PayeeId",
                principalTable: "Payees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
