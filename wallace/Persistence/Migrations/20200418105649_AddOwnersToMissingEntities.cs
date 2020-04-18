using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Wallace.Persistence.Migrations
{
    public partial class AddOwnersToMissingEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "OwnerId",
                table: "Transactions",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "OwnerId",
                table: "Payees",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_OwnerId",
                table: "Transactions",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Payees_OwnerId",
                table: "Payees",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payees_Users_OwnerId",
                table: "Payees",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Users_OwnerId",
                table: "Transactions",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payees_Users_OwnerId",
                table: "Payees");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Users_OwnerId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_OwnerId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Payees_OwnerId",
                table: "Payees");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Payees");
        }
    }
}
