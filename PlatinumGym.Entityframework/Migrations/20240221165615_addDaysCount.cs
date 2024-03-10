using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlatinumGymPro.Migrations
{
    /// <inheritdoc />
    public partial class addDaysCount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DaysCount",
                table: "Subscriptions",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DaysCount",
                table: "Subscriptions");
        }
    }
}
