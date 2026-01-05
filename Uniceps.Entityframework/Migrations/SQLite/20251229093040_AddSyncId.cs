using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Uniceps.Entityframework.Migrations.SQLite
{
    /// <inheritdoc />
    public partial class AddSyncId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Credit_Employees_EmpPersonId",
                table: "Credit");

            migrationBuilder.DropForeignKey(
                name: "FK_Metrics_Players_PlayerId",
                table: "Metrics");

            migrationBuilder.AddColumn<Guid>(
                name: "SyncId",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SyncId",
                table: "SyncObjects",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "PlayerSyncId",
                table: "Subscriptions",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SportSyncId",
                table: "Subscriptions",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SyncId",
                table: "Subscriptions",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "TrainerSyncId",
                table: "Subscriptions",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SyncId",
                table: "Sports",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SyncId",
                table: "Players",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "PlayerSyncId",
                table: "PlayerPayments",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SubscriptionSyncId",
                table: "PlayerPayments",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SyncId",
                table: "PlayerPayments",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SyncId",
                table: "PayReferance",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<int>(
                name: "PlayerId",
                table: "Metrics",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PlayerSyncId",
                table: "Metrics",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SyncId",
                table: "Metrics",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SyncId",
                table: "Expenses",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SyncId",
                table: "Employees",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "PlayerSyncId",
                table: "DailyPlayerReport",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SubscriptionSyncId",
                table: "DailyPlayerReport",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SyncId",
                table: "DailyPlayerReport",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<int>(
                name: "EmpPersonId",
                table: "Credit",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "EmpPersonSyncId",
                table: "Credit",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SyncId",
                table: "Credit",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SyncId",
                table: "authenticationLogs",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_SyncId",
                table: "Subscriptions",
                column: "SyncId");

            migrationBuilder.CreateIndex(
                name: "IX_Sports_SyncId",
                table: "Sports",
                column: "SyncId");

            migrationBuilder.CreateIndex(
                name: "IX_Players_SyncId",
                table: "Players",
                column: "SyncId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_SyncId",
                table: "Employees",
                column: "SyncId");

            migrationBuilder.AddForeignKey(
                name: "FK_Credit_Employees_EmpPersonId",
                table: "Credit",
                column: "EmpPersonId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Metrics_Players_PlayerId",
                table: "Metrics",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Credit_Employees_EmpPersonId",
                table: "Credit");

            migrationBuilder.DropForeignKey(
                name: "FK_Metrics_Players_PlayerId",
                table: "Metrics");

            migrationBuilder.DropIndex(
                name: "IX_Subscriptions_SyncId",
                table: "Subscriptions");

            migrationBuilder.DropIndex(
                name: "IX_Sports_SyncId",
                table: "Sports");

            migrationBuilder.DropIndex(
                name: "IX_Players_SyncId",
                table: "Players");

            migrationBuilder.DropIndex(
                name: "IX_Employees_SyncId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "SyncId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "SyncId",
                table: "SyncObjects");

            migrationBuilder.DropColumn(
                name: "PlayerSyncId",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "SportSyncId",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "SyncId",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "TrainerSyncId",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "SyncId",
                table: "Sports");

            migrationBuilder.DropColumn(
                name: "SyncId",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "PlayerSyncId",
                table: "PlayerPayments");

            migrationBuilder.DropColumn(
                name: "SubscriptionSyncId",
                table: "PlayerPayments");

            migrationBuilder.DropColumn(
                name: "SyncId",
                table: "PlayerPayments");

            migrationBuilder.DropColumn(
                name: "SyncId",
                table: "PayReferance");

            migrationBuilder.DropColumn(
                name: "PlayerSyncId",
                table: "Metrics");

            migrationBuilder.DropColumn(
                name: "SyncId",
                table: "Metrics");

            migrationBuilder.DropColumn(
                name: "SyncId",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "SyncId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "PlayerSyncId",
                table: "DailyPlayerReport");

            migrationBuilder.DropColumn(
                name: "SubscriptionSyncId",
                table: "DailyPlayerReport");

            migrationBuilder.DropColumn(
                name: "SyncId",
                table: "DailyPlayerReport");

            migrationBuilder.DropColumn(
                name: "EmpPersonSyncId",
                table: "Credit");

            migrationBuilder.DropColumn(
                name: "SyncId",
                table: "Credit");

            migrationBuilder.DropColumn(
                name: "SyncId",
                table: "authenticationLogs");

            migrationBuilder.AlterColumn<int>(
                name: "PlayerId",
                table: "Metrics",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "EmpPersonId",
                table: "Credit",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_Credit_Employees_EmpPersonId",
                table: "Credit",
                column: "EmpPersonId",
                principalTable: "Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Metrics_Players_PlayerId",
                table: "Metrics",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "Id");
        }
    }
}
