using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Unicepse.Entityframework.Migrations.SqliteMigrations
{
    /// <inheritdoc />
    public partial class RemovePrivateProps : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsMoved",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "IsPlayerPay",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "IsPrivate",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "PrivatePrice",
                table: "Subscriptions");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsMoved",
                table: "Subscriptions",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPlayerPay",
                table: "Subscriptions",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPrivate",
                table: "Subscriptions",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "PrivatePrice",
                table: "Subscriptions",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
