using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Unicepse.Entityframework.Migrations
{
    /// <inheritdoc />
    public partial class addDataStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DataStatus",
                table: "Subscriptions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DataStatus",
                table: "Players",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DataStatus",
                table: "PlayerRoutine",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DataStatus",
                table: "PlayerPayments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DataStatus",
                table: "Metrics",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DataStatus",
                table: "DailyPlayerReport",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataStatus",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "DataStatus",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "DataStatus",
                table: "PlayerRoutine");

            migrationBuilder.DropColumn(
                name: "DataStatus",
                table: "PlayerPayments");

            migrationBuilder.DropColumn(
                name: "DataStatus",
                table: "Metrics");

            migrationBuilder.DropColumn(
                name: "DataStatus",
                table: "DailyPlayerReport");
        }
    }
}
