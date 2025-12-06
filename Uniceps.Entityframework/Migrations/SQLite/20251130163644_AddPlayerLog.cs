using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Uniceps.Entityframework.Migrations.SQLite
{
    /// <inheritdoc />
    public partial class AddPlayerLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DailyPlayerReport_Players_PlayerId",
                table: "DailyPlayerReport");

            migrationBuilder.DropIndex(
                name: "IX_DailyPlayerReport_PlayerId",
                table: "DailyPlayerReport");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Subscriptions",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PlayerId",
                table: "DailyPlayerReport",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "DailyPlayerReport",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PlayerName",
                table: "DailyPlayerReport",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SportName",
                table: "DailyPlayerReport",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "SubscriptionId",
                table: "DailyPlayerReport",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "DailyPlayerReport");

            migrationBuilder.DropColumn(
                name: "PlayerName",
                table: "DailyPlayerReport");

            migrationBuilder.DropColumn(
                name: "SportName",
                table: "DailyPlayerReport");

            migrationBuilder.DropColumn(
                name: "SubscriptionId",
                table: "DailyPlayerReport");

            migrationBuilder.AlterColumn<int>(
                name: "PlayerId",
                table: "DailyPlayerReport",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.CreateIndex(
                name: "IX_DailyPlayerReport_PlayerId",
                table: "DailyPlayerReport",
                column: "PlayerId");

            migrationBuilder.AddForeignKey(
                name: "FK_DailyPlayerReport_Players_PlayerId",
                table: "DailyPlayerReport",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "Id");
        }
    }
}
