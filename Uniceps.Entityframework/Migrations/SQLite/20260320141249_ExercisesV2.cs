using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Uniceps.Entityframework.Migrations.SQLite
{
    /// <inheritdoc />
    public partial class ExercisesV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoutineItemModels_Exercises_ExerciseId",
                table: "RoutineItemModels");

            migrationBuilder.DropIndex(
                name: "IX_RoutineItemModels_ExerciseId",
                table: "RoutineItemModels");

            migrationBuilder.AddColumn<string>(
                name: "ExerciseV2Id",
                table: "RoutineItemModels",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ExercisesV2",
                columns: table => new
                {
                    ExerciseId = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    MuscleGroupCode = table.Column<string>(type: "TEXT", nullable: false),
                    MuscleHeadCode = table.Column<string>(type: "TEXT", nullable: false),
                    EquipmentCode = table.Column<string>(type: "TEXT", nullable: false),
                    MuscleAux1 = table.Column<string>(type: "TEXT", nullable: true),
                    MuscleAux2 = table.Column<string>(type: "TEXT", nullable: true),
                    MuscleAux3 = table.Column<string>(type: "TEXT", nullable: true),
                    Mechanism = table.Column<int>(type: "INTEGER", nullable: false),
                    Version = table.Column<int>(type: "INTEGER", nullable: false),
                    ImagePath = table.Column<string>(type: "TEXT", nullable: true),
                    IsLegacy = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExercisesV2", x => x.ExerciseId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RoutineItemModels_ExerciseV2Id",
                table: "RoutineItemModels",
                column: "ExerciseV2Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RoutineItemModels_ExercisesV2_ExerciseV2Id",
                table: "RoutineItemModels",
                column: "ExerciseV2Id",
                principalTable: "ExercisesV2",
                principalColumn: "ExerciseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoutineItemModels_ExercisesV2_ExerciseV2Id",
                table: "RoutineItemModels");

            migrationBuilder.DropTable(
                name: "ExercisesV2");

            migrationBuilder.DropIndex(
                name: "IX_RoutineItemModels_ExerciseV2Id",
                table: "RoutineItemModels");

            migrationBuilder.DropColumn(
                name: "ExerciseV2Id",
                table: "RoutineItemModels");

            migrationBuilder.CreateIndex(
                name: "IX_RoutineItemModels_ExerciseId",
                table: "RoutineItemModels",
                column: "ExerciseId");

            migrationBuilder.AddForeignKey(
                name: "FK_RoutineItemModels_Exercises_ExerciseId",
                table: "RoutineItemModels",
                column: "ExerciseId",
                principalTable: "Exercises",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
