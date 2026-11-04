using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Uniceps.Entityframework.Migrations.SqlServer
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
                name: "IsSubscribed",
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
                newName: "IsDeleted");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdatedUtc",
                table: "Users",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "ServerVersion",
                table: "Users",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "SyncObjects",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdatedUtc",
                table: "SyncObjects",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "ServerVersion",
                table: "SyncObjects",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Subscriptions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdatedUtc",
                table: "Subscriptions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "ServerVersion",
                table: "Subscriptions",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Sports",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdatedUtc",
                table: "Sports",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "ServerVersion",
                table: "Sports",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "RoutineModels",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdatedUtc",
                table: "RoutineModels",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "ServerVersion",
                table: "RoutineModels",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "ServerVersion",
                table: "Players",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "PlayerPayments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdatedUtc",
                table: "PlayerPayments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "ServerVersion",
                table: "PlayerPayments",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "PayReferance",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdatedUtc",
                table: "PayReferance",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "ServerVersion",
                table: "PayReferance",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "OtherRevenues",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdatedUtc",
                table: "OtherRevenues",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "ServerVersion",
                table: "OtherRevenues",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Metrics",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdatedUtc",
                table: "Metrics",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "ServerVersion",
                table: "Metrics",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Expenses",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdatedUtc",
                table: "Expenses",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "ServerVersion",
                table: "Expenses",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Employees",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdatedUtc",
                table: "Employees",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "ServerVersion",
                table: "Employees",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "EmployeeAccounts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdatedUtc",
                table: "EmployeeAccounts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "ServerVersion",
                table: "EmployeeAccounts",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "DailyPlayerReport",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdatedUtc",
                table: "DailyPlayerReport",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "ServerVersion",
                table: "DailyPlayerReport",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Credit",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdatedUtc",
                table: "Credit",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "ServerVersion",
                table: "Credit",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "authenticationLogs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdatedUtc",
                table: "authenticationLogs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "ServerVersion",
                table: "authenticationLogs",
                type: "bigint",
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
                name: "ServerVersion",
                table: "Players");

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
                name: "LastUpdatedUtc",
                table: "Players",
                newName: "SubscribeEndDate");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "Players",
                newName: "IsTakenContainer");

            migrationBuilder.AddColumn<int>(
                name: "DataStatus",
                table: "Players",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsSubscribed",
                table: "Players",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "SubscribeDate",
                table: "Players",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
