using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlatinumGymPro.Migrations
{
    /// <inheritdoc />
    public partial class _2ndinitial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Sportid",
                table: "Players",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DailyPlayerReport",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    loginTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    logoutTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PlayerId = table.Column<int>(type: "int", nullable: true),
                    IsTakenKey = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyPlayerReport", x => x.id);
                    table.ForeignKey(
                        name: "FK_DailyPlayerReport_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Offer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Parcent = table.Column<int>(type: "int", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SubscribeCount = table.Column<int>(type: "int", nullable: false),
                    Fullpay = table.Column<bool>(type: "bit", nullable: false),
                    IsBoth = table.Column<bool>(type: "bit", nullable: false),
                    IsTrainer = table.Column<bool>(type: "bit", nullable: false),
                    IsGym = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Offer", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sport",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<double>(type: "float", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    DaysInWeek = table.Column<int>(type: "int", nullable: false),
                    DailyPrice = table.Column<double>(type: "float", nullable: false),
                    DaysCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sport", x => x.id);
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
                name: "Employee",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Salary = table.Column<bool>(type: "bit", nullable: false),
                    Parcent = table.Column<bool>(type: "bit", nullable: false),
                    SalaryValue = table.Column<double>(type: "float", nullable: false),
                    ParcentValue = table.Column<int>(type: "int", nullable: false),
                    IsSecrtaria = table.Column<bool>(type: "bit", nullable: false),
                    Position = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Balance = table.Column<double>(type: "float", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsTrainer = table.Column<bool>(type: "bit", nullable: false),
                    Sportid = table.Column<int>(type: "int", nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BirthDate = table.Column<int>(type: "int", nullable: false),
                    GenderMale = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employee", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employee_Sport_Sportid",
                        column: x => x.Sportid,
                        principalTable: "Sport",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "PlayerProgram",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Sportid = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerProgram", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayerProgram_Sport_Sportid",
                        column: x => x.Sportid,
                        principalTable: "Sport",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Training",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CategoryId = table.Column<int>(type: "int", nullable: true),
                    Muscle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Sportid = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Training", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Training_Sport_Sportid",
                        column: x => x.Sportid,
                        principalTable: "Sport",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Training_TrainingCategory_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "TrainingCategory",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Credit",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmpPersonId = table.Column<int>(type: "int", nullable: true),
                    CreditValue = table.Column<double>(type: "float", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Credit", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Credit_Employee_EmpPersonId",
                        column: x => x.EmpPersonId,
                        principalTable: "Employee",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Expenses",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value = table.Column<double>(type: "float", nullable: false),
                    date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    isManager = table.Column<bool>(type: "bit", nullable: false),
                    RecipientId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Expenses", x => x.id);
                    table.ForeignKey(
                        name: "FK_Expenses_Employee_RecipientId",
                        column: x => x.RecipientId,
                        principalTable: "Employee",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PlayerTrainings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Sportid = table.Column<int>(type: "int", nullable: true),
                    LastCheck = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TrainerId = table.Column<int>(type: "int", nullable: true),
                    PrevTrainerId = table.Column<int>(name: "PrevTrainer_Id", type: "int", nullable: false),
                    PlayerId = table.Column<int>(type: "int", nullable: true),
                    RollDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    OfferId = table.Column<int>(type: "int", nullable: true),
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
                    table.PrimaryKey("PK_PlayerTrainings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayerTrainings_Employee_TrainerId",
                        column: x => x.TrainerId,
                        principalTable: "Employee",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PlayerTrainings_Offer_OfferId",
                        column: x => x.OfferId,
                        principalTable: "Offer",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PlayerTrainings_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PlayerTrainings_Sport_Sportid",
                        column: x => x.Sportid,
                        principalTable: "Sport",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsAdmin = table.Column<bool>(type: "bit", nullable: false),
                    EmployeeId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employee",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TrainingProgram",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TrainingsId = table.Column<int>(type: "int", nullable: true),
                    CategoryId = table.Column<int>(type: "int", nullable: true),
                    Counter = table.Column<int>(type: "int", nullable: false),
                    Rounds = table.Column<int>(type: "int", nullable: false),
                    PlayerProgramId = table.Column<int>(type: "int", nullable: true)
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

            migrationBuilder.CreateTable(
                name: "PayReferance",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlayerId = table.Column<int>(type: "int", nullable: true),
                    PlayerTrainingId = table.Column<int>(type: "int", nullable: true),
                    Value = table.Column<double>(type: "float", nullable: false),
                    RefDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayReferance", x => x.id);
                    table.ForeignKey(
                        name: "FK_PayReferance_PlayerTrainings_PlayerTrainingId",
                        column: x => x.PlayerTrainingId,
                        principalTable: "PlayerTrainings",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PayReferance_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PlayerPayments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlayerId = table.Column<int>(type: "int", nullable: true),
                    PaymentValue = table.Column<double>(type: "float", nullable: false),
                    RecipientId = table.Column<int>(type: "int", nullable: true),
                    Des = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PayDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TrainingId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerPayments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayerPayments_Employee_RecipientId",
                        column: x => x.RecipientId,
                        principalTable: "Employee",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PlayerPayments_PlayerTrainings_TrainingId",
                        column: x => x.TrainingId,
                        principalTable: "PlayerTrainings",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PlayerPayments_Players_PlayerId",
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
                        name: "FK_TrainerDueses_Employee_TrainerId",
                        column: x => x.TrainerId,
                        principalTable: "Employee",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TrainerDueses_PlayerPayments_PlayerPaymentId",
                        column: x => x.PlayerPaymentId,
                        principalTable: "PlayerPayments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TrainerDueses_PlayerTrainings_PlayerTrainingId",
                        column: x => x.PlayerTrainingId,
                        principalTable: "PlayerTrainings",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Players_Sportid",
                table: "Players",
                column: "Sportid");

            migrationBuilder.CreateIndex(
                name: "IX_Credit_EmpPersonId",
                table: "Credit",
                column: "EmpPersonId");

            migrationBuilder.CreateIndex(
                name: "IX_DailyPlayerReport_PlayerId",
                table: "DailyPlayerReport",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_Sportid",
                table: "Employee",
                column: "Sportid");

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_RecipientId",
                table: "Expenses",
                column: "RecipientId");

            migrationBuilder.CreateIndex(
                name: "IX_PayReferance_PlayerId",
                table: "PayReferance",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_PayReferance_PlayerTrainingId",
                table: "PayReferance",
                column: "PlayerTrainingId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerPayments_PlayerId",
                table: "PlayerPayments",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerPayments_RecipientId",
                table: "PlayerPayments",
                column: "RecipientId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerPayments_TrainingId",
                table: "PlayerPayments",
                column: "TrainingId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerProgram_Sportid",
                table: "PlayerProgram",
                column: "Sportid");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerTrainings_OfferId",
                table: "PlayerTrainings",
                column: "OfferId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerTrainings_PlayerId",
                table: "PlayerTrainings",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerTrainings_Sportid",
                table: "PlayerTrainings",
                column: "Sportid");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerTrainings_TrainerId",
                table: "PlayerTrainings",
                column: "TrainerId");

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
                name: "IX_Training_Sportid",
                table: "Training",
                column: "Sportid");

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

            migrationBuilder.CreateIndex(
                name: "IX_Users_EmployeeId",
                table: "Users",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Players_Sport_Sportid",
                table: "Players",
                column: "Sportid",
                principalTable: "Sport",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Players_Sport_Sportid",
                table: "Players");

            migrationBuilder.DropTable(
                name: "Credit");

            migrationBuilder.DropTable(
                name: "DailyPlayerReport");

            migrationBuilder.DropTable(
                name: "Expenses");

            migrationBuilder.DropTable(
                name: "PayReferance");

            migrationBuilder.DropTable(
                name: "TrainerDueses");

            migrationBuilder.DropTable(
                name: "TrainingProgram");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "PlayerPayments");

            migrationBuilder.DropTable(
                name: "PlayerProgram");

            migrationBuilder.DropTable(
                name: "Training");

            migrationBuilder.DropTable(
                name: "PlayerTrainings");

            migrationBuilder.DropTable(
                name: "TrainingCategory");

            migrationBuilder.DropTable(
                name: "Employee");

            migrationBuilder.DropTable(
                name: "Offer");

            migrationBuilder.DropTable(
                name: "Sport");

            migrationBuilder.DropIndex(
                name: "IX_Players_Sportid",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "Sportid",
                table: "Players");
        }
    }
}
