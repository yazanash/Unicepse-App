using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Uniceps.Entityframework.Migrations.SQLite
{
    /// <inheritdoc />
    public partial class ApplyChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_SyncId",
                table: "Subscriptions",
                column: "SyncId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sports_SyncId",
                table: "Sports",
                column: "SyncId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RoutineModels_SyncId",
                table: "RoutineModels",
                column: "SyncId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Players_SyncId",
                table: "Players",
                column: "SyncId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlayerPayments_SyncId",
                table: "PlayerPayments",
                column: "SyncId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Metrics_SyncId",
                table: "Metrics",
                column: "SyncId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_SyncId",
                table: "Expenses",
                column: "SyncId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_SyncId",
                table: "Employees",
                column: "SyncId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Credit_SyncId",
                table: "Credit",
                column: "SyncId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Subscriptions_SyncId",
                table: "Subscriptions");

            migrationBuilder.DropIndex(
                name: "IX_Sports_SyncId",
                table: "Sports");

            migrationBuilder.DropIndex(
                name: "IX_RoutineModels_SyncId",
                table: "RoutineModels");

            migrationBuilder.DropIndex(
                name: "IX_Players_SyncId",
                table: "Players");

            migrationBuilder.DropIndex(
                name: "IX_PlayerPayments_SyncId",
                table: "PlayerPayments");

            migrationBuilder.DropIndex(
                name: "IX_Metrics_SyncId",
                table: "Metrics");

            migrationBuilder.DropIndex(
                name: "IX_Expenses_SyncId",
                table: "Expenses");

            migrationBuilder.DropIndex(
                name: "IX_Employees_SyncId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Credit_SyncId",
                table: "Credit");

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
        }
    }
}
