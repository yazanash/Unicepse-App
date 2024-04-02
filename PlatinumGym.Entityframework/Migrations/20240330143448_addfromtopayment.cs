using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlatinumGymPro.Migrations
{
    /// <inheritdoc />
    public partial class addfromtopayment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CoverDays",
                table: "PlayerPayments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "From",
                table: "PlayerPayments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CoverDays",
                table: "PlayerPayments");

            migrationBuilder.DropColumn(
                name: "From",
                table: "PlayerPayments");
        }
    }
}
