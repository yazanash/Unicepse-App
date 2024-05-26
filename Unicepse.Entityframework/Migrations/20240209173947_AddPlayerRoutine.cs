using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Unicepse.Entityframework.Migrations
{
    /// <inheritdoc />
    public partial class AddPlayerRoutine : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Exercises",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Group = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Muscel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exercises", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlayerRoutine",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoutineNo = table.Column<int>(type: "int", nullable: false),
                    PlayerId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerRoutine", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayerRoutine_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TrainerDueses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TrainerId = table.Column<int>(type: "int", nullable: true),
                    PlayerPaymentId = table.Column<int>(type: "int", nullable: true),
                    PlayerTrainingId = table.Column<int>(type: "int", nullable: true),
                    Value = table.Column<double>(type: "float", nullable: false),
                    IsPrivate = table.Column<bool>(type: "bit", nullable: false),
                    From = table.Column<DateTime>(type: "datetime2", nullable: false),
                    To = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainerDueses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrainerDueses_Employees_TrainerId",
                        column: x => x.TrainerId,
                        principalTable: "Employees",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TrainerDueses_PlayerPayments_PlayerPaymentId",
                        column: x => x.PlayerPaymentId,
                        principalTable: "PlayerPayments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TrainerDueses_Subscriptions_PlayerTrainingId",
                        column: x => x.PlayerTrainingId,
                        principalTable: "Subscriptions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RoutineItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExerciseId = table.Column<int>(type: "int", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Orders = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PlayerRoutineId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoutineItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoutineItems_Exercises_ExerciseId",
                        column: x => x.ExerciseId,
                        principalTable: "Exercises",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RoutineItems_PlayerRoutine_PlayerRoutineId",
                        column: x => x.PlayerRoutineId,
                        principalTable: "PlayerRoutine",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlayerRoutine_PlayerId",
                table: "PlayerRoutine",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_RoutineItems_ExerciseId",
                table: "RoutineItems",
                column: "ExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_RoutineItems_PlayerRoutineId",
                table: "RoutineItems",
                column: "PlayerRoutineId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainerDueses_PlayerPaymentId",
                table: "TrainerDueses",
                column: "PlayerPaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainerDueses_PlayerTrainingId",
                table: "TrainerDueses",
                column: "PlayerTrainingId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainerDueses_TrainerId",
                table: "TrainerDueses",
                column: "TrainerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoutineItems");

            migrationBuilder.DropTable(
                name: "TrainerDueses");

            migrationBuilder.DropTable(
                name: "Exercises");

            migrationBuilder.DropTable(
                name: "PlayerRoutine");
        }
    }
}
