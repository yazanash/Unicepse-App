using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Unicepse.Entityframework.Migrations
{
    /// <inheritdoc />
    public partial class AddMetrics : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Metrics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlayerId = table.Column<int>(type: "int", nullable: true),
                    Hieght = table.Column<double>(type: "float", nullable: true),
                    Wieght = table.Column<double>(type: "float", nullable: true),
                    L_Arm = table.Column<double>(type: "float", nullable: true),
                    R_Arm = table.Column<double>(type: "float", nullable: true),
                    L_Humerus = table.Column<double>(type: "float", nullable: true),
                    R_Humerus = table.Column<double>(type: "float", nullable: true),
                    L_Thigh = table.Column<double>(type: "float", nullable: true),
                    R_Thigh = table.Column<double>(type: "float", nullable: true),
                    L_Leg = table.Column<double>(type: "float", nullable: true),
                    R_Leg = table.Column<double>(type: "float", nullable: true),
                    Nick = table.Column<double>(type: "float", nullable: true),
                    Shoulders = table.Column<double>(type: "float", nullable: true),
                    Waist = table.Column<double>(type: "float", nullable: true),
                    Chest = table.Column<double>(type: "float", nullable: true),
                    Hips = table.Column<double>(type: "float", nullable: true),
                    CheckDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Metrics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Metrics_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Metrics_PlayerId",
                table: "Metrics",
                column: "PlayerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Metrics");
        }
    }
}
