﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Unicepse.Entityframework.DbContexts;

#nullable disable

namespace Unicepse.Entityframework.Migrations
{
    [DbContext(typeof(PlatinumGymDbContext))]
    partial class PlatinumGymDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("EmployeeSport", b =>
                {
                    b.Property<int>("SportsId")
                        .HasColumnType("int");

                    b.Property<int>("TrainersId")
                        .HasColumnType("int");

                    b.HasKey("SportsId", "TrainersId");

                    b.HasIndex("TrainersId");

                    b.ToTable("EmployeeSport");
                });

            modelBuilder.Entity("Unicepse.Core.Models.Authentication.AuthenticationLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("LoginDateTime")
                        .HasColumnType("datetime2");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.Property<bool>("status")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("authenticationLogs");
                });

            modelBuilder.Entity("Unicepse.Core.Models.Authentication.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("Disable")
                        .HasColumnType("bit");

                    b.Property<string>("OwnerName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(4000)");

                    b.Property<string>("Position")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(4000)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Unicepse.Core.Models.DailyActivity.DailyPlayerReport", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("DataStatus")
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsLogged")
                        .HasColumnType("bit");

                    b.Property<bool>("IsTakenKey")
                        .HasColumnType("bit");

                    b.Property<int?>("PlayerId")
                        .HasColumnType("int");

                    b.Property<int?>("SubscriptionId")
                        .HasColumnType("int");

                    b.Property<DateTime>("loginTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("logoutTime")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("PlayerId");

                    b.HasIndex("SubscriptionId");

                    b.ToTable("DailyPlayerReport");
                });

            modelBuilder.Entity("Unicepse.Core.Models.Employee.Credit", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<double>("CreditValue")
                        .HasColumnType("float");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("EmpPersonId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("EmpPersonId");

                    b.ToTable("Credit");
                });

            modelBuilder.Entity("Unicepse.Core.Models.Employee.Employee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<double>("Balance")
                        .HasColumnType("float");

                    b.Property<int>("BirthDate")
                        .HasColumnType("int");

                    b.Property<string>("FullName")
                        .HasColumnType("nvarchar(4000)");

                    b.Property<bool>("GenderMale")
                        .HasColumnType("bit");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsSecrtaria")
                        .HasColumnType("bit");

                    b.Property<bool>("IsTrainer")
                        .HasColumnType("bit");

                    b.Property<bool>("Parcent")
                        .HasColumnType("bit");

                    b.Property<int>("ParcentValue")
                        .HasColumnType("int");

                    b.Property<string>("Phone")
                        .HasColumnType("nvarchar(4000)");

                    b.Property<string>("Position")
                        .HasColumnType("nvarchar(4000)");

                    b.Property<bool>("Salary")
                        .HasColumnType("bit");

                    b.Property<double>("SalaryValue")
                        .HasColumnType("float");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("Unicepse.Core.Models.Employee.TrainerDueses", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CountSubscription")
                        .HasColumnType("int");

                    b.Property<double>("Credits")
                        .HasColumnType("float");

                    b.Property<double>("CreditsCount")
                        .HasColumnType("float");

                    b.Property<DateTime>("IssueDate")
                        .HasColumnType("datetime2");

                    b.Property<double>("Parcent")
                        .HasColumnType("float");

                    b.Property<double>("Salary")
                        .HasColumnType("float");

                    b.Property<double>("TotalSubscriptions")
                        .HasColumnType("float");

                    b.Property<int?>("TrainerId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("TrainerId");

                    b.ToTable("TrainerDueses");
                });

            modelBuilder.Entity("Unicepse.Core.Models.Expenses.Expenses", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(4000)");

                    b.Property<int?>("RecipientId")
                        .HasColumnType("int");

                    b.Property<double>("Value")
                        .HasColumnType("float");

                    b.Property<DateTime>("date")
                        .HasColumnType("datetime2");

                    b.Property<bool>("isManager")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("RecipientId");

                    b.ToTable("Expenses");
                });

            modelBuilder.Entity("Unicepse.Core.Models.GymProfile", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("GymId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("GymName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Logo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OwnerName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Telephone")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("GymProfile");
                });

            modelBuilder.Entity("Unicepse.Core.Models.License", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("GymId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LicenseId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Plan")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Price")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("SubscribeDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("SubscribeEndDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Token")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Licenses");
                });

            modelBuilder.Entity("Unicepse.Core.Models.Metric.Metric", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CheckDate")
                        .HasColumnType("datetime2");

                    b.Property<double>("Chest")
                        .HasColumnType("float");

                    b.Property<int>("DataStatus")
                        .HasColumnType("int");

                    b.Property<double>("Hieght")
                        .HasColumnType("float");

                    b.Property<double>("Hips")
                        .HasColumnType("float");

                    b.Property<double>("L_Arm")
                        .HasColumnType("float");

                    b.Property<double>("L_Humerus")
                        .HasColumnType("float");

                    b.Property<double>("L_Leg")
                        .HasColumnType("float");

                    b.Property<double>("L_Thigh")
                        .HasColumnType("float");

                    b.Property<double>("Nick")
                        .HasColumnType("float");

                    b.Property<int?>("PlayerId")
                        .HasColumnType("int");

                    b.Property<double>("R_Arm")
                        .HasColumnType("float");

                    b.Property<double>("R_Humerus")
                        .HasColumnType("float");

                    b.Property<double>("R_Leg")
                        .HasColumnType("float");

                    b.Property<double>("R_Thigh")
                        .HasColumnType("float");

                    b.Property<double>("Shoulders")
                        .HasColumnType("float");

                    b.Property<double>("Waist")
                        .HasColumnType("float");

                    b.Property<double>("Wieght")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("PlayerId");

                    b.ToTable("Metrics");
                });

            modelBuilder.Entity("Unicepse.Core.Models.Payment.PayReferance", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("PlayerId")
                        .HasColumnType("int");

                    b.Property<int?>("PlayerTrainingId")
                        .HasColumnType("int");

                    b.Property<DateTime>("RefDate")
                        .HasColumnType("datetime2");

                    b.Property<double>("Value")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("PlayerId");

                    b.HasIndex("PlayerTrainingId");

                    b.ToTable("PayReferance");
                });

            modelBuilder.Entity("Unicepse.Core.Models.Payment.PlayerPayment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CoverDays")
                        .HasColumnType("int");

                    b.Property<int>("DataStatus")
                        .HasColumnType("int");

                    b.Property<string>("Des")
                        .HasColumnType("nvarchar(4000)");

                    b.Property<DateTime>("From")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("PayDate")
                        .HasColumnType("datetime2");

                    b.Property<double>("PaymentValue")
                        .HasColumnType("float");

                    b.Property<int?>("PlayerId")
                        .HasColumnType("int");

                    b.Property<int?>("RecipientId")
                        .HasColumnType("int");

                    b.Property<int?>("SubscriptionId")
                        .HasColumnType("int");

                    b.Property<DateTime>("To")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("PlayerId");

                    b.HasIndex("RecipientId");

                    b.HasIndex("SubscriptionId");

                    b.ToTable("PlayerPayments");
                });

            modelBuilder.Entity("Unicepse.Core.Models.Player.Player", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<double>("Balance")
                        .HasColumnType("float");

                    b.Property<int>("BirthDate")
                        .HasColumnType("int");

                    b.Property<int>("DataStatus")
                        .HasColumnType("int");

                    b.Property<string>("FullName")
                        .HasColumnType("nvarchar(4000)");

                    b.Property<bool>("GenderMale")
                        .HasColumnType("bit");

                    b.Property<double>("Hieght")
                        .HasColumnType("float");

                    b.Property<bool>("IsSubscribed")
                        .HasColumnType("bit");

                    b.Property<bool>("IsTakenContainer")
                        .HasColumnType("bit");

                    b.Property<string>("Phone")
                        .HasColumnType("nvarchar(4000)");

                    b.Property<DateTime>("SubscribeDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("SubscribeEndDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("UID")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Weight")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("Players");
                });

            modelBuilder.Entity("Unicepse.Core.Models.Sport.Sport", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<double>("DailyPrice")
                        .HasColumnType("float");

                    b.Property<int>("DaysCount")
                        .HasColumnType("int");

                    b.Property<int>("DaysInWeek")
                        .HasColumnType("int");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(4000)");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("Sports");
                });

            modelBuilder.Entity("Unicepse.Core.Models.Subscription.Subscription", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("DataStatus")
                        .HasColumnType("int");

                    b.Property<int>("DaysCount")
                        .HasColumnType("int");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsMoved")
                        .HasColumnType("bit");

                    b.Property<bool>("IsPaid")
                        .HasColumnType("bit");

                    b.Property<bool>("IsPlayerPay")
                        .HasColumnType("bit");

                    b.Property<bool>("IsPrivate")
                        .HasColumnType("bit");

                    b.Property<bool>("IsStopped")
                        .HasColumnType("bit");

                    b.Property<DateTime>("LastCheck")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("LastPaid")
                        .HasColumnType("datetime2");

                    b.Property<int>("MonthCount")
                        .HasColumnType("int");

                    b.Property<string>("OfferDes")
                        .HasColumnType("nvarchar(4000)");

                    b.Property<double>("OfferValue")
                        .HasColumnType("float");

                    b.Property<double>("PaidValue")
                        .HasColumnType("float");

                    b.Property<int?>("PlayerId")
                        .HasColumnType("int");

                    b.Property<int>("PrevTrainer_Id")
                        .HasColumnType("int");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.Property<double>("PriceAfterOffer")
                        .HasColumnType("float");

                    b.Property<double>("PrivatePrice")
                        .HasColumnType("float");

                    b.Property<double>("RestValue")
                        .HasColumnType("float");

                    b.Property<DateTime>("RollDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("SportId")
                        .HasColumnType("int");

                    b.Property<int?>("TrainerId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PlayerId");

                    b.HasIndex("SportId");

                    b.HasIndex("TrainerId");

                    b.ToTable("Subscriptions");
                });

            modelBuilder.Entity("Unicepse.Core.Models.TrainingProgram.Exercises", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("GroupId")
                        .HasColumnType("int");

                    b.Property<string>("ImageId")
                        .HasColumnType("nvarchar(4000)");

                    b.Property<string>("Muscel")
                        .HasColumnType("nvarchar(4000)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(4000)");

                    b.Property<int>("Tid")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Exercises");
                });

            modelBuilder.Entity("Unicepse.Core.Models.TrainingProgram.PlayerRoutine", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("DataStatus")
                        .HasColumnType("int");

                    b.Property<string>("DaysGroupMap")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsTemplate")
                        .HasColumnType("bit");

                    b.Property<int?>("PlayerId")
                        .HasColumnType("int");

                    b.Property<DateTime>("RoutineData")
                        .HasColumnType("datetime2");

                    b.Property<string>("RoutineNo")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("PlayerId");

                    b.ToTable("PlayerRoutine");
                });

            modelBuilder.Entity("Unicepse.Core.Models.TrainingProgram.RoutineItems", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("ExercisesId")
                        .HasColumnType("int");

                    b.Property<int>("ItemOrder")
                        .HasColumnType("int");

                    b.Property<string>("Notes")
                        .HasColumnType("nvarchar(4000)");

                    b.Property<string>("Orders")
                        .HasColumnType("nvarchar(4000)");

                    b.Property<int?>("PlayerRoutineId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ExercisesId");

                    b.HasIndex("PlayerRoutineId");

                    b.ToTable("RoutineItems");
                });

            modelBuilder.Entity("EmployeeSport", b =>
                {
                    b.HasOne("Unicepse.Core.Models.Sport.Sport", null)
                        .WithMany()
                        .HasForeignKey("SportsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Unicepse.Core.Models.Employee.Employee", null)
                        .WithMany()
                        .HasForeignKey("TrainersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Unicepse.Core.Models.Authentication.AuthenticationLog", b =>
                {
                    b.HasOne("Unicepse.Core.Models.Authentication.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Unicepse.Core.Models.DailyActivity.DailyPlayerReport", b =>
                {
                    b.HasOne("Unicepse.Core.Models.Player.Player", "Player")
                        .WithMany()
                        .HasForeignKey("PlayerId");

                    b.HasOne("Unicepse.Core.Models.Subscription.Subscription", "Subscription")
                        .WithMany()
                        .HasForeignKey("SubscriptionId");

                    b.Navigation("Player");

                    b.Navigation("Subscription");
                });

            modelBuilder.Entity("Unicepse.Core.Models.Employee.Credit", b =>
                {
                    b.HasOne("Unicepse.Core.Models.Employee.Employee", "EmpPerson")
                        .WithMany()
                        .HasForeignKey("EmpPersonId");

                    b.Navigation("EmpPerson");
                });

            modelBuilder.Entity("Unicepse.Core.Models.Employee.TrainerDueses", b =>
                {
                    b.HasOne("Unicepse.Core.Models.Employee.Employee", "Trainer")
                        .WithMany()
                        .HasForeignKey("TrainerId");

                    b.Navigation("Trainer");
                });

            modelBuilder.Entity("Unicepse.Core.Models.Expenses.Expenses", b =>
                {
                    b.HasOne("Unicepse.Core.Models.Employee.Employee", "Recipient")
                        .WithMany()
                        .HasForeignKey("RecipientId");

                    b.Navigation("Recipient");
                });

            modelBuilder.Entity("Unicepse.Core.Models.Metric.Metric", b =>
                {
                    b.HasOne("Unicepse.Core.Models.Player.Player", "Player")
                        .WithMany()
                        .HasForeignKey("PlayerId");

                    b.Navigation("Player");
                });

            modelBuilder.Entity("Unicepse.Core.Models.Payment.PayReferance", b =>
                {
                    b.HasOne("Unicepse.Core.Models.Player.Player", "Player")
                        .WithMany()
                        .HasForeignKey("PlayerId");

                    b.HasOne("Unicepse.Core.Models.Subscription.Subscription", "PlayerTraining")
                        .WithMany()
                        .HasForeignKey("PlayerTrainingId");

                    b.Navigation("Player");

                    b.Navigation("PlayerTraining");
                });

            modelBuilder.Entity("Unicepse.Core.Models.Payment.PlayerPayment", b =>
                {
                    b.HasOne("Unicepse.Core.Models.Player.Player", "Player")
                        .WithMany()
                        .HasForeignKey("PlayerId");

                    b.HasOne("Unicepse.Core.Models.Employee.Employee", "Recipient")
                        .WithMany()
                        .HasForeignKey("RecipientId");

                    b.HasOne("Unicepse.Core.Models.Subscription.Subscription", "Subscription")
                        .WithMany("Payments")
                        .HasForeignKey("SubscriptionId");

                    b.Navigation("Player");

                    b.Navigation("Recipient");

                    b.Navigation("Subscription");
                });

            modelBuilder.Entity("Unicepse.Core.Models.Subscription.Subscription", b =>
                {
                    b.HasOne("Unicepse.Core.Models.Player.Player", "Player")
                        .WithMany()
                        .HasForeignKey("PlayerId");

                    b.HasOne("Unicepse.Core.Models.Sport.Sport", "Sport")
                        .WithMany("PlayerTrainings")
                        .HasForeignKey("SportId");

                    b.HasOne("Unicepse.Core.Models.Employee.Employee", "Trainer")
                        .WithMany("PlayerTrainings")
                        .HasForeignKey("TrainerId");

                    b.Navigation("Player");

                    b.Navigation("Sport");

                    b.Navigation("Trainer");
                });

            modelBuilder.Entity("Unicepse.Core.Models.TrainingProgram.PlayerRoutine", b =>
                {
                    b.HasOne("Unicepse.Core.Models.Player.Player", "Player")
                        .WithMany()
                        .HasForeignKey("PlayerId");

                    b.Navigation("Player");
                });

            modelBuilder.Entity("Unicepse.Core.Models.TrainingProgram.RoutineItems", b =>
                {
                    b.HasOne("Unicepse.Core.Models.TrainingProgram.Exercises", "Exercises")
                        .WithMany()
                        .HasForeignKey("ExercisesId");

                    b.HasOne("Unicepse.Core.Models.TrainingProgram.PlayerRoutine", "PlayerRoutine")
                        .WithMany("RoutineSchedule")
                        .HasForeignKey("PlayerRoutineId");

                    b.Navigation("Exercises");

                    b.Navigation("PlayerRoutine");
                });

            modelBuilder.Entity("Unicepse.Core.Models.Employee.Employee", b =>
                {
                    b.Navigation("PlayerTrainings");
                });

            modelBuilder.Entity("Unicepse.Core.Models.Sport.Sport", b =>
                {
                    b.Navigation("PlayerTrainings");
                });

            modelBuilder.Entity("Unicepse.Core.Models.Subscription.Subscription", b =>
                {
                    b.Navigation("Payments");
                });

            modelBuilder.Entity("Unicepse.Core.Models.TrainingProgram.PlayerRoutine", b =>
                {
                    b.Navigation("RoutineSchedule");
                });
#pragma warning restore 612, 618
        }
    }
}
