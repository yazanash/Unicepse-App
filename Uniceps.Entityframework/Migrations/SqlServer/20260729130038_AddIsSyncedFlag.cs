using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Uniceps.Entityframework.Migrations.SqlServer
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
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSynced",
                table: "SyncObjects",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSynced",
                table: "Subscriptions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSynced",
                table: "Sports",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSynced",
                table: "RoutineModels",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSynced",
                table: "Players",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSynced",
                table: "PlayerPayments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSynced",
                table: "PayReferance",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSynced",
                table: "OtherRevenues",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSynced",
                table: "Metrics",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSynced",
                table: "Expenses",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSynced",
                table: "Employees",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSynced",
                table: "EmployeeAccounts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSynced",
                table: "DailyPlayerReport",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSynced",
                table: "Credit",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSynced",
                table: "authenticationLogs",
                type: "bit",
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
