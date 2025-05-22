using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Unicepse.Entityframework.Migrations
{
    /// <inheritdoc />
    public partial class AddRoutineTempTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RoutineModels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Level = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoutineModels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DayGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RoutineId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DayGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DayGroups_RoutineModels_RoutineId",
                        column: x => x.RoutineId,
                        principalTable: "RoutineModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoutineItemModels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExerciseId = table.Column<int>(type: "int", nullable: false),
                    DayId = table.Column<int>(type: "int", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoutineItemModels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoutineItemModels_DayGroups_DayId",
                        column: x => x.DayId,
                        principalTable: "DayGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoutineItemModels_Exercises_ExerciseId",
                        column: x => x.ExerciseId,
                        principalTable: "Exercises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SetModels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoundIndex = table.Column<int>(type: "int", nullable: false),
                    Repetition = table.Column<int>(type: "int", nullable: false),
                    RoutineItemId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SetModels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SetModels_RoutineItemModels_RoutineItemId",
                        column: x => x.RoutineItemId,
                        principalTable: "RoutineItemModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DayGroups_RoutineId",
                table: "DayGroups",
                column: "RoutineId");

            migrationBuilder.CreateIndex(
                name: "IX_RoutineItemModels_DayId",
                table: "RoutineItemModels",
                column: "DayId");

            migrationBuilder.CreateIndex(
                name: "IX_RoutineItemModels_ExerciseId",
                table: "RoutineItemModels",
                column: "ExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_SetModels_RoutineItemId",
                table: "SetModels",
                column: "RoutineItemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SetModels");

            migrationBuilder.DropTable(
                name: "RoutineItemModels");

            migrationBuilder.DropTable(
                name: "DayGroups");

            migrationBuilder.DropTable(
                name: "RoutineModels");
        }
    }
}
