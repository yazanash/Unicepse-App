using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Unicepse.Entityframework.Migrations
{
    /// <inheritdoc />
    public partial class addSubsToAttendance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SubscriptionId",
                table: "DailyPlayerReport",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DailyPlayerReport_SubscriptionId",
                table: "DailyPlayerReport",
                column: "SubscriptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_DailyPlayerReport_Subscriptions_SubscriptionId",
                table: "DailyPlayerReport",
                column: "SubscriptionId",
                principalTable: "Subscriptions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DailyPlayerReport_Subscriptions_SubscriptionId",
                table: "DailyPlayerReport");

            migrationBuilder.DropIndex(
                name: "IX_DailyPlayerReport_SubscriptionId",
                table: "DailyPlayerReport");

            migrationBuilder.DropColumn(
                name: "SubscriptionId",
                table: "DailyPlayerReport");
        }
    }
}
