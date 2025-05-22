using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Unicepse.Entityframework.Migrations.SqliteMigrations
{
    /// <inheritdoc />
    public partial class removeRec : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayerPayments_Employees_RecipientId",
                table: "PlayerPayments");

            migrationBuilder.DropIndex(
                name: "IX_PlayerPayments_RecipientId",
                table: "PlayerPayments");

            migrationBuilder.DropColumn(
                name: "RecipientId",
                table: "PlayerPayments");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RecipientId",
                table: "PlayerPayments",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlayerPayments_RecipientId",
                table: "PlayerPayments",
                column: "RecipientId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerPayments_Employees_RecipientId",
                table: "PlayerPayments",
                column: "RecipientId",
                principalTable: "Employees",
                principalColumn: "Id");
        }
    }
}
