using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Uniceps.Entityframework.Migrations.SQLite
{
    /// <inheritdoc />
    public partial class AddServerVersion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataStatus",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "SubscribeDate",
                table: "Players");

            migrationBuilder.RenameColumn(
                name: "SubscribeEndDate",
                table: "Players",
                newName: "LastUpdatedUtc");

            migrationBuilder.RenameColumn(
                name: "IsTakenContainer",
                table: "Players",
                newName: "ServerVersion");

            migrationBuilder.RenameColumn(
                name: "IsSubscribed",
                table: "Players",
                newName: "IsDeleted");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Users",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdatedUtc",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "ServerVersion",
                table: "Users",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "SyncObjects",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdatedUtc",
                table: "SyncObjects",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "ServerVersion",
                table: "SyncObjects",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Subscriptions",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdatedUtc",
                table: "Subscriptions",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "ServerVersion",
                table: "Subscriptions",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Sports",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdatedUtc",
                table: "Sports",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "ServerVersion",
                table: "Sports",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "RoutineModels",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdatedUtc",
                table: "RoutineModels",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "ServerVersion",
                table: "RoutineModels",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "PlayerPayments",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdatedUtc",
                table: "PlayerPayments",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "ServerVersion",
                table: "PlayerPayments",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "PayReferance",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdatedUtc",
                table: "PayReferance",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "ServerVersion",
                table: "PayReferance",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "OtherRevenues",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdatedUtc",
                table: "OtherRevenues",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "ServerVersion",
                table: "OtherRevenues",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Metrics",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdatedUtc",
                table: "Metrics",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "ServerVersion",
                table: "Metrics",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Expenses",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdatedUtc",
                table: "Expenses",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "ServerVersion",
                table: "Expenses",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Employees",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdatedUtc",
                table: "Employees",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "ServerVersion",
                table: "Employees",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "EmployeeAccounts",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdatedUtc",
                table: "EmployeeAccounts",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "ServerVersion",
                table: "EmployeeAccounts",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "DailyPlayerReport",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdatedUtc",
                table: "DailyPlayerReport",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "ServerVersion",
                table: "DailyPlayerReport",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Credit",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdatedUtc",
                table: "Credit",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "ServerVersion",
                table: "Credit",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "authenticationLogs",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdatedUtc",
                table: "authenticationLogs",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "ServerVersion",
                table: "authenticationLogs",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LastUpdatedUtc",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ServerVersion",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "SyncObjects");

            migrationBuilder.DropColumn(
                name: "LastUpdatedUtc",
                table: "SyncObjects");

            migrationBuilder.DropColumn(
                name: "ServerVersion",
                table: "SyncObjects");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "LastUpdatedUtc",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "ServerVersion",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Sports");

            migrationBuilder.DropColumn(
                name: "LastUpdatedUtc",
                table: "Sports");

            migrationBuilder.DropColumn(
                name: "ServerVersion",
                table: "Sports");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "RoutineModels");

            migrationBuilder.DropColumn(
                name: "LastUpdatedUtc",
                table: "RoutineModels");

            migrationBuilder.DropColumn(
                name: "ServerVersion",
                table: "RoutineModels");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "PlayerPayments");

            migrationBuilder.DropColumn(
                name: "LastUpdatedUtc",
                table: "PlayerPayments");

            migrationBuilder.DropColumn(
                name: "ServerVersion",
                table: "PlayerPayments");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "PayReferance");

            migrationBuilder.DropColumn(
                name: "LastUpdatedUtc",
                table: "PayReferance");

            migrationBuilder.DropColumn(
                name: "ServerVersion",
                table: "PayReferance");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "OtherRevenues");

            migrationBuilder.DropColumn(
                name: "LastUpdatedUtc",
                table: "OtherRevenues");

            migrationBuilder.DropColumn(
                name: "ServerVersion",
                table: "OtherRevenues");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Metrics");

            migrationBuilder.DropColumn(
                name: "LastUpdatedUtc",
                table: "Metrics");

            migrationBuilder.DropColumn(
                name: "ServerVersion",
                table: "Metrics");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "LastUpdatedUtc",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "ServerVersion",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "LastUpdatedUtc",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "ServerVersion",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "EmployeeAccounts");

            migrationBuilder.DropColumn(
                name: "LastUpdatedUtc",
                table: "EmployeeAccounts");

            migrationBuilder.DropColumn(
                name: "ServerVersion",
                table: "EmployeeAccounts");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "DailyPlayerReport");

            migrationBuilder.DropColumn(
                name: "LastUpdatedUtc",
                table: "DailyPlayerReport");

            migrationBuilder.DropColumn(
                name: "ServerVersion",
                table: "DailyPlayerReport");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Credit");

            migrationBuilder.DropColumn(
                name: "LastUpdatedUtc",
                table: "Credit");

            migrationBuilder.DropColumn(
                name: "ServerVersion",
                table: "Credit");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "authenticationLogs");

            migrationBuilder.DropColumn(
                name: "LastUpdatedUtc",
                table: "authenticationLogs");

            migrationBuilder.DropColumn(
                name: "ServerVersion",
                table: "authenticationLogs");

            migrationBuilder.RenameColumn(
                name: "ServerVersion",
                table: "Players",
                newName: "IsTakenContainer");

            migrationBuilder.RenameColumn(
                name: "LastUpdatedUtc",
                table: "Players",
                newName: "SubscribeEndDate");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "Players",
                newName: "IsSubscribed");

            migrationBuilder.AddColumn<int>(
                name: "DataStatus",
                table: "Players",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "SubscribeDate",
                table: "Players",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
