using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Uniceps.Entityframework.Migrations.SQLite
{
    /// <inheritdoc />
    public partial class AddIsSyncedFlag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSynced",
                table: "Users",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSynced",
                table: "SyncObjects",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSynced",
                table: "Subscriptions",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSynced",
                table: "Sports",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSynced",
                table: "RoutineModels",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSynced",
                table: "Players",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSynced",
                table: "PlayerPayments",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSynced",
                table: "PayReferance",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSynced",
                table: "OtherRevenues",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSynced",
                table: "Metrics",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSynced",
                table: "Expenses",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSynced",
                table: "Employees",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSynced",
                table: "EmployeeAccounts",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSynced",
                table: "DailyPlayerReport",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSynced",
                table: "Credit",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSynced",
                table: "authenticationLogs",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSynced",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsSynced",
                table: "SyncObjects");

            migrationBuilder.DropColumn(
                name: "IsSynced",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "IsSynced",
                table: "Sports");

            migrationBuilder.DropColumn(
                name: "IsSynced",
                table: "RoutineModels");

            migrationBuilder.DropColumn(
                name: "IsSynced",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "IsSynced",
                table: "PlayerPayments");

            migrationBuilder.DropColumn(
                name: "IsSynced",
                table: "PayReferance");

            migrationBuilder.DropColumn(
                name: "IsSynced",
                table: "OtherRevenues");

            migrationBuilder.DropColumn(
                name: "IsSynced",
                table: "Metrics");

            migrationBuilder.DropColumn(
                name: "IsSynced",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "IsSynced",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "IsSynced",
                table: "EmployeeAccounts");

            migrationBuilder.DropColumn(
                name: "IsSynced",
                table: "DailyPlayerReport");

            migrationBuilder.DropColumn(
                name: "IsSynced",
                table: "Credit");

            migrationBuilder.DropColumn(
                name: "IsSynced",
                table: "authenticationLogs");
        }
    }
}
