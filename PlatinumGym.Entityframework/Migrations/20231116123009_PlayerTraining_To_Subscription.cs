using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlatinumGymPro.Migrations
{
    /// <inheritdoc />
    public partial class PlayerTraining_To_Subscription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PayReferance_PlayerTrainings_PlayerTrainingId",
                table: "PayReferance");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerPayments_PlayerTrainings_TrainingId",
                table: "PlayerPayments");

            migrationBuilder.DropForeignKey(
                name: "FK_TrainerDueses_PlayerTrainings_PlayerTrainingId",
                table: "TrainerDueses");

            migrationBuilder.DropTable(
                name: "PlayerTrainings");

            migrationBuilder.RenameColumn(
                name: "TrainingId",
                table: "PlayerPayments",
                newName: "SubscriptionId");

            migrationBuilder.RenameIndex(
                name: "IX_PlayerPayments_TrainingId",
                table: "PlayerPayments",
                newName: "IX_PlayerPayments_SubscriptionId");

            migrationBuilder.CreateTable(
                name: "Subscriptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SportId = table.Column<int>(type: "int", nullable: true),
                    LastCheck = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TrainerId = table.Column<int>(type: "int", nullable: true),
                    PrevTrainer_Id = table.Column<int>(type: "int", nullable: false),
                    PlayerId = table.Column<int>(type: "int", nullable: true),
                    RollDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    OfferValue = table.Column<double>(type: "float", nullable: false),
                    OfferDes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PriceAfterOffer = table.Column<double>(type: "float", nullable: false),
                    MonthCount = table.Column<int>(type: "int", nullable: false),
                    IsPrivate = table.Column<bool>(type: "bit", nullable: false),
                    IsPlayerPay = table.Column<bool>(type: "bit", nullable: false),
                    IsStopped = table.Column<bool>(type: "bit", nullable: false),
                    IsMoved = table.Column<bool>(type: "bit", nullable: false),
                    PrivatePrice = table.Column<double>(type: "float", nullable: false),
                    IsPaid = table.Column<bool>(type: "bit", nullable: false),
                    PaidValue = table.Column<double>(type: "float", nullable: false),
                    RestValue = table.Column<double>(type: "float", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastPaid = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subscriptions_Employees_TrainerId",
                        column: x => x.TrainerId,
                        principalTable: "Employees",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Subscriptions_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Subscriptions_Sports_SportId",
                        column: x => x.SportId,
                        principalTable: "Sports",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_PlayerId",
                table: "Subscriptions",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_SportId",
                table: "Subscriptions",
                column: "SportId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_TrainerId",
                table: "Subscriptions",
                column: "TrainerId");

            migrationBuilder.AddForeignKey(
                name: "FK_PayReferance_Subscriptions_PlayerTrainingId",
                table: "PayReferance",
                column: "PlayerTrainingId",
                principalTable: "Subscriptions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerPayments_Subscriptions_SubscriptionId",
                table: "PlayerPayments",
                column: "SubscriptionId",
                principalTable: "Subscriptions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TrainerDueses_Subscriptions_PlayerTrainingId",
                table: "TrainerDueses",
                column: "PlayerTrainingId",
                principalTable: "Subscriptions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PayReferance_Subscriptions_PlayerTrainingId",
                table: "PayReferance");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerPayments_Subscriptions_SubscriptionId",
                table: "PlayerPayments");

            migrationBuilder.DropForeignKey(
                name: "FK_TrainerDueses_Subscriptions_PlayerTrainingId",
                table: "TrainerDueses");

            migrationBuilder.DropTable(
                name: "Subscriptions");

            migrationBuilder.RenameColumn(
                name: "SubscriptionId",
                table: "PlayerPayments",
                newName: "TrainingId");

            migrationBuilder.RenameIndex(
                name: "IX_PlayerPayments_SubscriptionId",
                table: "PlayerPayments",
                newName: "IX_PlayerPayments_TrainingId");

            migrationBuilder.CreateTable(
                name: "PlayerTrainings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlayerId = table.Column<int>(type: "int", nullable: true),
                    SportId = table.Column<int>(type: "int", nullable: true),
                    TrainerId = table.Column<int>(type: "int", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsMoved = table.Column<bool>(type: "bit", nullable: false),
                    IsPaid = table.Column<bool>(type: "bit", nullable: false),
                    IsPlayerPay = table.Column<bool>(type: "bit", nullable: false),
                    IsPrivate = table.Column<bool>(type: "bit", nullable: false),
                    IsStopped = table.Column<bool>(type: "bit", nullable: false),
                    LastCheck = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastPaid = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MonthCount = table.Column<int>(type: "int", nullable: false),
                    OfferDes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OfferValue = table.Column<double>(type: "float", nullable: false),
                    PaidValue = table.Column<double>(type: "float", nullable: false),
                    PrevTrainer_Id = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    PriceAfterOffer = table.Column<double>(type: "float", nullable: false),
                    PrivatePrice = table.Column<double>(type: "float", nullable: false),
                    RestValue = table.Column<double>(type: "float", nullable: false),
                    RollDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerTrainings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayerTrainings_Employees_TrainerId",
                        column: x => x.TrainerId,
                        principalTable: "Employees",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PlayerTrainings_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PlayerTrainings_Sports_SportId",
                        column: x => x.SportId,
                        principalTable: "Sports",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlayerTrainings_PlayerId",
                table: "PlayerTrainings",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerTrainings_SportId",
                table: "PlayerTrainings",
                column: "SportId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerTrainings_TrainerId",
                table: "PlayerTrainings",
                column: "TrainerId");

            migrationBuilder.AddForeignKey(
                name: "FK_PayReferance_PlayerTrainings_PlayerTrainingId",
                table: "PayReferance",
                column: "PlayerTrainingId",
                principalTable: "PlayerTrainings",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerPayments_PlayerTrainings_TrainingId",
                table: "PlayerPayments",
                column: "TrainingId",
                principalTable: "PlayerTrainings",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TrainerDueses_PlayerTrainings_PlayerTrainingId",
                table: "TrainerDueses",
                column: "PlayerTrainingId",
                principalTable: "PlayerTrainings",
                principalColumn: "Id");
        }
    }
}
