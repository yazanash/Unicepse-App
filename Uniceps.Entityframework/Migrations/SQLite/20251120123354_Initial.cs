using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Uniceps.Entityframework.Migrations.SQLite
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Salary = table.Column<bool>(type: "INTEGER", nullable: false),
                    Parcent = table.Column<bool>(type: "INTEGER", nullable: false),
                    SalaryValue = table.Column<double>(type: "REAL", nullable: false),
                    ParcentValue = table.Column<int>(type: "INTEGER", nullable: false),
                    IsSecrtaria = table.Column<bool>(type: "INTEGER", nullable: false),
                    Position = table.Column<string>(type: "TEXT", nullable: true),
                    StartDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Balance = table.Column<double>(type: "REAL", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsTrainer = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    FullName = table.Column<string>(type: "TEXT", nullable: true),
                    Phone = table.Column<string>(type: "TEXT", nullable: true),
                    BirthDate = table.Column<int>(type: "INTEGER", nullable: false),
                    GenderMale = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Exercises",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Tid = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    MuscleGroupId = table.Column<int>(type: "INTEGER", nullable: false),
                    Muscel = table.Column<string>(type: "TEXT", nullable: true),
                    ImagePath = table.Column<string>(type: "TEXT", nullable: true),
                    Version = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exercises", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MuscleGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    EngName = table.Column<string>(type: "TEXT", nullable: true),
                    PublicId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MuscleGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Weight = table.Column<double>(type: "REAL", nullable: false),
                    Hieght = table.Column<double>(type: "REAL", nullable: false),
                    SubscribeDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    SubscribeEndDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsTakenContainer = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsSubscribed = table.Column<bool>(type: "INTEGER", nullable: false),
                    UID = table.Column<string>(type: "TEXT", nullable: true),
                    DataStatus = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    FullName = table.Column<string>(type: "TEXT", nullable: true),
                    Phone = table.Column<string>(type: "TEXT", nullable: true),
                    BirthDate = table.Column<int>(type: "INTEGER", nullable: false),
                    GenderMale = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RoutineModels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Level = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoutineModels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Price = table.Column<double>(type: "REAL", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    DaysInWeek = table.Column<int>(type: "INTEGER", nullable: false),
                    DaysCount = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sports", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SyncObjects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OperationType = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityType = table.Column<int>(type: "INTEGER", nullable: false),
                    ObjectData = table.Column<string>(type: "TEXT", nullable: true),
                    Timestamp = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SyncObjects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SystemProfiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    BusinessId = table.Column<string>(type: "TEXT", nullable: true),
                    DisplayName = table.Column<string>(type: "TEXT", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: false),
                    Address = table.Column<string>(type: "TEXT", nullable: false),
                    OwnerName = table.Column<string>(type: "TEXT", nullable: false),
                    ProfileImagePath = table.Column<string>(type: "TEXT", nullable: true),
                    LocalProfileImagePath = table.Column<string>(type: "TEXT", nullable: true),
                    Gender = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemProfiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SystemSubscriptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PublicId = table.Column<Guid>(type: "TEXT", nullable: false),
                    PlanName = table.Column<string>(type: "TEXT", nullable: true),
                    Price = table.Column<decimal>(type: "TEXT", nullable: false),
                    StartDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EndDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsGift = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemSubscriptions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserName = table.Column<string>(type: "TEXT", nullable: true),
                    Password = table.Column<string>(type: "TEXT", nullable: true),
                    Position = table.Column<string>(type: "TEXT", nullable: true),
                    OwnerName = table.Column<string>(type: "TEXT", nullable: true),
                    Role = table.Column<int>(type: "INTEGER", nullable: false),
                    Disable = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Credit",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EmpPersonId = table.Column<int>(type: "INTEGER", nullable: true),
                    CreditValue = table.Column<double>(type: "REAL", nullable: false),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Credit", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Credit_Employees_EmpPersonId",
                        column: x => x.EmpPersonId,
                        principalTable: "Employees",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Expenses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    Value = table.Column<double>(type: "REAL", nullable: false),
                    date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    isManager = table.Column<bool>(type: "INTEGER", nullable: false),
                    RecipientId = table.Column<int>(type: "INTEGER", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Expenses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Expenses_Employees_RecipientId",
                        column: x => x.RecipientId,
                        principalTable: "Employees",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DailyPlayerReport",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    loginTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    logoutTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    PlayerId = table.Column<int>(type: "INTEGER", nullable: true),
                    KeyNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    IsTakenKey = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsLogged = table.Column<bool>(type: "INTEGER", nullable: false),
                    DataStatus = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyPlayerReport", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DailyPlayerReport_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Metrics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PlayerId = table.Column<int>(type: "INTEGER", nullable: true),
                    DataStatus = table.Column<int>(type: "INTEGER", nullable: false),
                    Hieght = table.Column<double>(type: "REAL", nullable: false),
                    Wieght = table.Column<double>(type: "REAL", nullable: false),
                    L_Arm = table.Column<double>(type: "REAL", nullable: false),
                    R_Arm = table.Column<double>(type: "REAL", nullable: false),
                    L_Humerus = table.Column<double>(type: "REAL", nullable: false),
                    R_Humerus = table.Column<double>(type: "REAL", nullable: false),
                    L_Thigh = table.Column<double>(type: "REAL", nullable: false),
                    R_Thigh = table.Column<double>(type: "REAL", nullable: false),
                    L_Leg = table.Column<double>(type: "REAL", nullable: false),
                    R_Leg = table.Column<double>(type: "REAL", nullable: false),
                    Nick = table.Column<double>(type: "REAL", nullable: false),
                    Shoulders = table.Column<double>(type: "REAL", nullable: false),
                    Waist = table.Column<double>(type: "REAL", nullable: false),
                    Chest = table.Column<double>(type: "REAL", nullable: false),
                    Hips = table.Column<double>(type: "REAL", nullable: false),
                    CheckDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "DayGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    RoutineId = table.Column<int>(type: "INTEGER", nullable: false),
                    Order = table.Column<int>(type: "INTEGER", nullable: false)
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
                name: "EmployeeSport",
                columns: table => new
                {
                    SportsId = table.Column<int>(type: "INTEGER", nullable: false),
                    TrainersId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeSport", x => new { x.SportsId, x.TrainersId });
                    table.ForeignKey(
                        name: "FK_EmployeeSport_Employees_TrainersId",
                        column: x => x.TrainersId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeSport_Sports_SportsId",
                        column: x => x.SportsId,
                        principalTable: "Sports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Subscriptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PlayerId = table.Column<int>(type: "INTEGER", nullable: false),
                    SportId = table.Column<int>(type: "INTEGER", nullable: false),
                    TrainerId = table.Column<int>(type: "INTEGER", nullable: true),
                    PlayerName = table.Column<string>(type: "TEXT", nullable: true),
                    SportName = table.Column<string>(type: "TEXT", nullable: true),
                    TrainerName = table.Column<string>(type: "TEXT", nullable: true),
                    LastCheck = table.Column<DateTime>(type: "TEXT", nullable: false),
                    RollDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Price = table.Column<double>(type: "REAL", nullable: false),
                    OfferValue = table.Column<double>(type: "REAL", nullable: false),
                    OfferDes = table.Column<string>(type: "TEXT", nullable: true),
                    PriceAfterOffer = table.Column<double>(type: "REAL", nullable: false),
                    MonthCount = table.Column<int>(type: "INTEGER", nullable: false),
                    DaysCount = table.Column<int>(type: "INTEGER", nullable: false),
                    IsStopped = table.Column<bool>(type: "INTEGER", nullable: false),
                    EndDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsRenewed = table.Column<bool>(type: "INTEGER", nullable: false),
                    EmployeeId = table.Column<int>(type: "INTEGER", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subscriptions_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Subscriptions_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Subscriptions_Sports_SportId",
                        column: x => x.SportId,
                        principalTable: "Sports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "authenticationLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: true),
                    LoginDateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    status = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_authenticationLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_authenticationLogs_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RoutineItemModels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ExerciseId = table.Column<int>(type: "INTEGER", nullable: false),
                    DayId = table.Column<int>(type: "INTEGER", nullable: false),
                    Order = table.Column<int>(type: "INTEGER", nullable: false)
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
                name: "PayReferance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PlayerId = table.Column<int>(type: "INTEGER", nullable: true),
                    PlayerTrainingId = table.Column<int>(type: "INTEGER", nullable: true),
                    Value = table.Column<double>(type: "REAL", nullable: false),
                    RefDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayReferance", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PayReferance_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PayReferance_Subscriptions_PlayerTrainingId",
                        column: x => x.PlayerTrainingId,
                        principalTable: "Subscriptions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PlayerPayments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PlayerId = table.Column<int>(type: "INTEGER", nullable: false),
                    PaymentValue = table.Column<double>(type: "REAL", nullable: false),
                    Des = table.Column<string>(type: "TEXT", nullable: true),
                    PayDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    SubscriptionId = table.Column<int>(type: "INTEGER", nullable: false),
                    CoveredFrom = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CoveredTo = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerPayments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayerPayments_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlayerPayments_Subscriptions_SubscriptionId",
                        column: x => x.SubscriptionId,
                        principalTable: "Subscriptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SetModels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RoundIndex = table.Column<int>(type: "INTEGER", nullable: false),
                    Repetition = table.Column<int>(type: "INTEGER", nullable: false),
                    RoutineItemId = table.Column<int>(type: "INTEGER", nullable: false)
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
                name: "IX_authenticationLogs_UserId",
                table: "authenticationLogs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Credit_EmpPersonId",
                table: "Credit",
                column: "EmpPersonId");

            migrationBuilder.CreateIndex(
                name: "IX_DailyPlayerReport_PlayerId",
                table: "DailyPlayerReport",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_DayGroups_RoutineId",
                table: "DayGroups",
                column: "RoutineId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeSport_TrainersId",
                table: "EmployeeSport",
                column: "TrainersId");

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_RecipientId",
                table: "Expenses",
                column: "RecipientId");

            migrationBuilder.CreateIndex(
                name: "IX_Metrics_PlayerId",
                table: "Metrics",
                column: "PlayerId");

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
                name: "IX_PlayerPayments_SubscriptionId",
                table: "PlayerPayments",
                column: "SubscriptionId");

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

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_EmployeeId",
                table: "Subscriptions",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_PlayerId",
                table: "Subscriptions",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_SportId",
                table: "Subscriptions",
                column: "SportId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "authenticationLogs");

            migrationBuilder.DropTable(
                name: "Credit");

            migrationBuilder.DropTable(
                name: "DailyPlayerReport");

            migrationBuilder.DropTable(
                name: "EmployeeSport");

            migrationBuilder.DropTable(
                name: "Expenses");

            migrationBuilder.DropTable(
                name: "Metrics");

            migrationBuilder.DropTable(
                name: "MuscleGroups");

            migrationBuilder.DropTable(
                name: "PayReferance");

            migrationBuilder.DropTable(
                name: "PlayerPayments");

            migrationBuilder.DropTable(
                name: "SetModels");

            migrationBuilder.DropTable(
                name: "SyncObjects");

            migrationBuilder.DropTable(
                name: "SystemProfiles");

            migrationBuilder.DropTable(
                name: "SystemSubscriptions");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Subscriptions");

            migrationBuilder.DropTable(
                name: "RoutineItemModels");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Players");

            migrationBuilder.DropTable(
                name: "Sports");

            migrationBuilder.DropTable(
                name: "DayGroups");

            migrationBuilder.DropTable(
                name: "Exercises");

            migrationBuilder.DropTable(
                name: "RoutineModels");
        }
    }
}
