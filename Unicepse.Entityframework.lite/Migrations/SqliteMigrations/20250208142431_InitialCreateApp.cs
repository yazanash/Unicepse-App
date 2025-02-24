using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Unicepse.Entityframework.lite.Migrations.SqliteMigrations
{
    /// <inheritdoc />
    public partial class InitialCreateApp : Migration
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
                    Position = table.Column<string>(type: "nvarchar(4000)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Balance = table.Column<double>(type: "REAL", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsTrainer = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(4000)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(4000)", nullable: true),
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
                    Name = table.Column<string>(type: "nvarchar(4000)", nullable: true),
                    GroupId = table.Column<int>(type: "INTEGER", nullable: false),
                    Muscel = table.Column<string>(type: "nvarchar(4000)", nullable: true),
                    ImageId = table.Column<string>(type: "nvarchar(4000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exercises", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GymProfile",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GymId = table.Column<string>(type: "TEXT", nullable: true),
                    GymName = table.Column<string>(type: "TEXT", nullable: true),
                    OwnerName = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: true),
                    Telephone = table.Column<string>(type: "TEXT", nullable: true),
                    Logo = table.Column<string>(type: "TEXT", nullable: true),
                    Address = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GymProfile", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Licenses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LicenseId = table.Column<string>(type: "TEXT", nullable: true),
                    GymId = table.Column<string>(type: "TEXT", nullable: true),
                    Plan = table.Column<string>(type: "TEXT", nullable: true),
                    SubscribeDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    SubscribeEndDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Token = table.Column<string>(type: "TEXT", nullable: true),
                    Price = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Licenses", x => x.Id);
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
                    Balance = table.Column<double>(type: "REAL", nullable: false),
                    UID = table.Column<string>(type: "TEXT", nullable: true),
                    DataStatus = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(4000)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(4000)", nullable: true),
                    BirthDate = table.Column<int>(type: "INTEGER", nullable: false),
                    GenderMale = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "nvarchar(4000)", nullable: true),
                    Price = table.Column<double>(type: "REAL", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    DaysInWeek = table.Column<int>(type: "INTEGER", nullable: false),
                    DailyPrice = table.Column<double>(type: "REAL", nullable: false),
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
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserName = table.Column<string>(type: "nvarchar(4000)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(4000)", nullable: true),
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
                    Description = table.Column<string>(type: "nvarchar(4000)", nullable: true),
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
                name: "TrainerDueses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TrainerId = table.Column<int>(type: "INTEGER", nullable: true),
                    TotalSubscriptions = table.Column<double>(type: "REAL", nullable: false),
                    CountSubscription = table.Column<int>(type: "INTEGER", nullable: false),
                    IssueDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Parcent = table.Column<double>(type: "REAL", nullable: false),
                    Credits = table.Column<double>(type: "REAL", nullable: false),
                    Salary = table.Column<double>(type: "REAL", nullable: false),
                    CreditsCount = table.Column<double>(type: "REAL", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainerDueses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrainerDueses_Employees_TrainerId",
                        column: x => x.TrainerId,
                        principalTable: "Employees",
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
                name: "PlayerRoutine",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RoutineNo = table.Column<string>(type: "TEXT", nullable: true),
                    RoutineData = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsTemplate = table.Column<bool>(type: "INTEGER", nullable: false),
                    PlayerId = table.Column<int>(type: "INTEGER", nullable: true),
                    DaysGroupMap = table.Column<string>(type: "TEXT", nullable: false),
                    DataStatus = table.Column<int>(type: "INTEGER", nullable: false)
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
                    SportId = table.Column<int>(type: "INTEGER", nullable: true),
                    LastCheck = table.Column<DateTime>(type: "TEXT", nullable: false),
                    TrainerId = table.Column<int>(type: "INTEGER", nullable: true),
                    PrevTrainer_Id = table.Column<int>(type: "INTEGER", nullable: false),
                    PlayerId = table.Column<int>(type: "INTEGER", nullable: true),
                    RollDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Price = table.Column<double>(type: "REAL", nullable: false),
                    OfferValue = table.Column<double>(type: "REAL", nullable: false),
                    OfferDes = table.Column<string>(type: "nvarchar(4000)", nullable: true),
                    PriceAfterOffer = table.Column<double>(type: "REAL", nullable: false),
                    MonthCount = table.Column<int>(type: "INTEGER", nullable: false),
                    DaysCount = table.Column<int>(type: "INTEGER", nullable: false),
                    IsPrivate = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsPlayerPay = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsStopped = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsMoved = table.Column<bool>(type: "INTEGER", nullable: false),
                    PrivatePrice = table.Column<double>(type: "REAL", nullable: false),
                    IsPaid = table.Column<bool>(type: "INTEGER", nullable: false),
                    PaidValue = table.Column<double>(type: "REAL", nullable: false),
                    RestValue = table.Column<double>(type: "REAL", nullable: false),
                    EndDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastPaid = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DataStatus = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
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
                name: "RoutineItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ExercisesId = table.Column<int>(type: "INTEGER", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(4000)", nullable: true),
                    Orders = table.Column<string>(type: "nvarchar(4000)", nullable: true),
                    ItemOrder = table.Column<int>(type: "INTEGER", nullable: false),
                    PlayerRoutineId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoutineItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoutineItems_Exercises_ExercisesId",
                        column: x => x.ExercisesId,
                        principalTable: "Exercises",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RoutineItems_PlayerRoutine_PlayerRoutineId",
                        column: x => x.PlayerRoutineId,
                        principalTable: "PlayerRoutine",
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
                    SubscriptionId = table.Column<int>(type: "INTEGER", nullable: true),
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
                    table.ForeignKey(
                        name: "FK_DailyPlayerReport_Subscriptions_SubscriptionId",
                        column: x => x.SubscriptionId,
                        principalTable: "Subscriptions",
                        principalColumn: "Id");
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
                    PlayerId = table.Column<int>(type: "INTEGER", nullable: true),
                    PaymentValue = table.Column<double>(type: "REAL", nullable: false),
                    RecipientId = table.Column<int>(type: "INTEGER", nullable: true),
                    Des = table.Column<string>(type: "nvarchar(4000)", nullable: true),
                    PayDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    From = table.Column<DateTime>(type: "TEXT", nullable: false),
                    To = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CoverDays = table.Column<int>(type: "INTEGER", nullable: false),
                    SubscriptionId = table.Column<int>(type: "INTEGER", nullable: true),
                    DataStatus = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerPayments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayerPayments_Employees_RecipientId",
                        column: x => x.RecipientId,
                        principalTable: "Employees",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PlayerPayments_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PlayerPayments_Subscriptions_SubscriptionId",
                        column: x => x.SubscriptionId,
                        principalTable: "Subscriptions",
                        principalColumn: "Id");
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
                name: "IX_DailyPlayerReport_SubscriptionId",
                table: "DailyPlayerReport",
                column: "SubscriptionId");

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
                name: "IX_PlayerPayments_RecipientId",
                table: "PlayerPayments",
                column: "RecipientId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerPayments_SubscriptionId",
                table: "PlayerPayments",
                column: "SubscriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerRoutine_PlayerId",
                table: "PlayerRoutine",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_RoutineItems_ExercisesId",
                table: "RoutineItems",
                column: "ExercisesId");

            migrationBuilder.CreateIndex(
                name: "IX_RoutineItems_PlayerRoutineId",
                table: "RoutineItems",
                column: "PlayerRoutineId");

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

            migrationBuilder.CreateIndex(
                name: "IX_TrainerDueses_TrainerId",
                table: "TrainerDueses",
                column: "TrainerId");
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
                name: "GymProfile");

            migrationBuilder.DropTable(
                name: "Licenses");

            migrationBuilder.DropTable(
                name: "Metrics");

            migrationBuilder.DropTable(
                name: "PayReferance");

            migrationBuilder.DropTable(
                name: "PlayerPayments");

            migrationBuilder.DropTable(
                name: "RoutineItems");

            migrationBuilder.DropTable(
                name: "SyncObjects");

            migrationBuilder.DropTable(
                name: "TrainerDueses");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Subscriptions");

            migrationBuilder.DropTable(
                name: "Exercises");

            migrationBuilder.DropTable(
                name: "PlayerRoutine");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Sports");

            migrationBuilder.DropTable(
                name: "Players");
        }
    }
}
