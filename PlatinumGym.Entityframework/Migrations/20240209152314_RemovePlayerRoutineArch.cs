using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlatinumGymPro.Migrations
{
    /// <inheritdoc />
    public partial class RemovePlayerRoutineArch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrainerDueses");

            migrationBuilder.DropTable(
                name: "TrainingProgram");

            migrationBuilder.DropTable(
                name: "PlayerProgram");

            migrationBuilder.DropTable(
                name: "Training");

            migrationBuilder.DropTable(
                name: "TrainingCategory");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PlayerProgram",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SportId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerProgram", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayerProgram_Sports_SportId",
                        column: x => x.SportId,
                        principalTable: "Sports",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TrainerDueses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlayerPaymentId = table.Column<int>(type: "int", nullable: true),
                    PlayerTrainingId = table.Column<int>(type: "int", nullable: true),
                    TrainerId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    From = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsPrivate = table.Column<bool>(type: "bit", nullable: false),
                    To = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Value = table.Column<double>(type: "float", nullable: false)
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
                name: "TrainingCategory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingCategory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Training",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryId = table.Column<int>(type: "int", nullable: true),
                    SportId = table.Column<int>(type: "int", nullable: true),
                    Muscle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Training", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Training_Sports_SportId",
                        column: x => x.SportId,
                        principalTable: "Sports",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Training_TrainingCategory_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "TrainingCategory",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TrainingProgram",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryId = table.Column<int>(type: "int", nullable: true),
                    PlayerProgramId = table.Column<int>(type: "int", nullable: true),
                    TrainingsId = table.Column<int>(type: "int", nullable: true),
                    Counter = table.Column<int>(type: "int", nullable: false),
                    Rounds = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingProgram", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrainingProgram_PlayerProgram_PlayerProgramId",
                        column: x => x.PlayerProgramId,
                        principalTable: "PlayerProgram",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TrainingProgram_TrainingCategory_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "TrainingCategory",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TrainingProgram_Training_TrainingsId",
                        column: x => x.TrainingsId,
                        principalTable: "Training",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlayerProgram_SportId",
                table: "PlayerProgram",
                column: "SportId");

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

            migrationBuilder.CreateIndex(
                name: "IX_Training_CategoryId",
                table: "Training",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Training_SportId",
                table: "Training",
                column: "SportId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingProgram_CategoryId",
                table: "TrainingProgram",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingProgram_PlayerProgramId",
                table: "TrainingProgram",
                column: "PlayerProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingProgram_TrainingsId",
                table: "TrainingProgram",
                column: "TrainingsId");
        }
    }
}
