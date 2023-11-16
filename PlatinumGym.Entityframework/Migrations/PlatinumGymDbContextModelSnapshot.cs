﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PlatinumGym.Entityframework.DbContexts;

#nullable disable

namespace PlatinumGymPro.Migrations
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

            modelBuilder.Entity("PlatinumGym.Core.Models.Authentication.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("EmployeeId")
                        .HasColumnType("int");

                    b.Property<bool>("IsAdmin")
                        .HasColumnType("bit");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("PlatinumGym.Core.Models.DailyActivity.DailyPlayerReport", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsTakenKey")
                        .HasColumnType("bit");

                    b.Property<int?>("PlayerId")
                        .HasColumnType("int");

                    b.Property<DateTime>("loginTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("logoutTime")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("PlayerId");

                    b.ToTable("DailyPlayerReport");
                });

            modelBuilder.Entity("PlatinumGym.Core.Models.Employee.Credit", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<double>("CreditValue")
                        .HasColumnType("float");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int?>("EmpPersonId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("EmpPersonId");

                    b.ToTable("Credit");
                });

            modelBuilder.Entity("PlatinumGym.Core.Models.Employee.Employee", b =>
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
                        .HasColumnType("nvarchar(max)");

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
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Position")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Salary")
                        .HasColumnType("bit");

                    b.Property<double>("SalaryValue")
                        .HasColumnType("float");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("PlatinumGym.Core.Models.Employee.TrainerDueses", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("From")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsPrivate")
                        .HasColumnType("bit");

                    b.Property<int?>("PlayerPaymentId")
                        .HasColumnType("int");

                    b.Property<int?>("PlayerTrainingId")
                        .HasColumnType("int");

                    b.Property<DateTime>("To")
                        .HasColumnType("datetime2");

                    b.Property<int?>("TrainerId")
                        .HasColumnType("int");

                    b.Property<double>("Value")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("PlayerPaymentId");

                    b.HasIndex("PlayerTrainingId");

                    b.HasIndex("TrainerId");

                    b.ToTable("TrainerDueses");
                });

            modelBuilder.Entity("PlatinumGym.Core.Models.Expenses.Expenses", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

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

            modelBuilder.Entity("PlatinumGym.Core.Models.Payment.PayReferance", b =>
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

            modelBuilder.Entity("PlatinumGym.Core.Models.Payment.PlayerPayment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Des")
                        .HasColumnType("nvarchar(max)");

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

                    b.HasKey("Id");

                    b.HasIndex("PlayerId");

                    b.HasIndex("RecipientId");

                    b.HasIndex("SubscriptionId");

                    b.ToTable("PlayerPayments");
                });

            modelBuilder.Entity("PlatinumGym.Core.Models.Player.Player", b =>
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
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("GenderMale")
                        .HasColumnType("bit");

                    b.Property<double>("Hieght")
                        .HasColumnType("float");

                    b.Property<bool>("IsSubscribed")
                        .HasColumnType("bit");

                    b.Property<bool>("IsTakenContainer")
                        .HasColumnType("bit");

                    b.Property<string>("Phone")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("SubscribeDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("SubscribeEndDate")
                        .HasColumnType("datetime2");

                    b.Property<double>("Weight")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("Players");
                });

            modelBuilder.Entity("PlatinumGym.Core.Models.Sport.Sport", b =>
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
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("Sports");
                });

            modelBuilder.Entity("PlatinumGym.Core.Models.Subscription.Subscription", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

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
                        .HasColumnType("nvarchar(max)");

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

            modelBuilder.Entity("PlatinumGym.Core.Models.TrainingProgram.PlayerProgram", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("SportId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SportId");

                    b.ToTable("PlayerProgram");
                });

            modelBuilder.Entity("PlatinumGym.Core.Models.TrainingProgram.Training", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("CategoryId")
                        .HasColumnType("int");

                    b.Property<string>("Muscle")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("SportId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("SportId");

                    b.ToTable("Training");
                });

            modelBuilder.Entity("PlatinumGym.Core.Models.TrainingProgram.TrainingCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("TrainingCategory");
                });

            modelBuilder.Entity("PlatinumGym.Core.Models.TrainingProgram.TrainingProgram", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("CategoryId")
                        .HasColumnType("int");

                    b.Property<int>("Counter")
                        .HasColumnType("int");

                    b.Property<int?>("PlayerProgramId")
                        .HasColumnType("int");

                    b.Property<int>("Rounds")
                        .HasColumnType("int");

                    b.Property<int?>("TrainingsId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("PlayerProgramId");

                    b.HasIndex("TrainingsId");

                    b.ToTable("TrainingProgram");
                });

            modelBuilder.Entity("EmployeeSport", b =>
                {
                    b.HasOne("PlatinumGym.Core.Models.Sport.Sport", null)
                        .WithMany()
                        .HasForeignKey("SportsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PlatinumGym.Core.Models.Employee.Employee", null)
                        .WithMany()
                        .HasForeignKey("TrainersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PlatinumGym.Core.Models.Authentication.User", b =>
                {
                    b.HasOne("PlatinumGym.Core.Models.Employee.Employee", "Employee")
                        .WithMany()
                        .HasForeignKey("EmployeeId");

                    b.Navigation("Employee");
                });

            modelBuilder.Entity("PlatinumGym.Core.Models.DailyActivity.DailyPlayerReport", b =>
                {
                    b.HasOne("PlatinumGym.Core.Models.Player.Player", "Player")
                        .WithMany()
                        .HasForeignKey("PlayerId");

                    b.Navigation("Player");
                });

            modelBuilder.Entity("PlatinumGym.Core.Models.Employee.Credit", b =>
                {
                    b.HasOne("PlatinumGym.Core.Models.Employee.Employee", "EmpPerson")
                        .WithMany()
                        .HasForeignKey("EmpPersonId");

                    b.Navigation("EmpPerson");
                });

            modelBuilder.Entity("PlatinumGym.Core.Models.Employee.TrainerDueses", b =>
                {
                    b.HasOne("PlatinumGym.Core.Models.Payment.PlayerPayment", "PlayerPayment")
                        .WithMany()
                        .HasForeignKey("PlayerPaymentId");

                    b.HasOne("PlatinumGym.Core.Models.Subscription.Subscription", "PlayerTraining")
                        .WithMany()
                        .HasForeignKey("PlayerTrainingId");

                    b.HasOne("PlatinumGym.Core.Models.Employee.Employee", "Trainer")
                        .WithMany()
                        .HasForeignKey("TrainerId");

                    b.Navigation("PlayerPayment");

                    b.Navigation("PlayerTraining");

                    b.Navigation("Trainer");
                });

            modelBuilder.Entity("PlatinumGym.Core.Models.Expenses.Expenses", b =>
                {
                    b.HasOne("PlatinumGym.Core.Models.Employee.Employee", "Recipient")
                        .WithMany()
                        .HasForeignKey("RecipientId");

                    b.Navigation("Recipient");
                });

            modelBuilder.Entity("PlatinumGym.Core.Models.Payment.PayReferance", b =>
                {
                    b.HasOne("PlatinumGym.Core.Models.Player.Player", "Player")
                        .WithMany()
                        .HasForeignKey("PlayerId");

                    b.HasOne("PlatinumGym.Core.Models.Subscription.Subscription", "PlayerTraining")
                        .WithMany()
                        .HasForeignKey("PlayerTrainingId");

                    b.Navigation("Player");

                    b.Navigation("PlayerTraining");
                });

            modelBuilder.Entity("PlatinumGym.Core.Models.Payment.PlayerPayment", b =>
                {
                    b.HasOne("PlatinumGym.Core.Models.Player.Player", "Player")
                        .WithMany()
                        .HasForeignKey("PlayerId");

                    b.HasOne("PlatinumGym.Core.Models.Employee.Employee", "Recipient")
                        .WithMany()
                        .HasForeignKey("RecipientId");

                    b.HasOne("PlatinumGym.Core.Models.Subscription.Subscription", "Subscription")
                        .WithMany()
                        .HasForeignKey("SubscriptionId");

                    b.Navigation("Player");

                    b.Navigation("Recipient");

                    b.Navigation("Subscription");
                });

            modelBuilder.Entity("PlatinumGym.Core.Models.Subscription.Subscription", b =>
                {
                    b.HasOne("PlatinumGym.Core.Models.Player.Player", "Player")
                        .WithMany()
                        .HasForeignKey("PlayerId");

                    b.HasOne("PlatinumGym.Core.Models.Sport.Sport", "Sport")
                        .WithMany("PlayerTrainings")
                        .HasForeignKey("SportId");

                    b.HasOne("PlatinumGym.Core.Models.Employee.Employee", "Trainer")
                        .WithMany()
                        .HasForeignKey("TrainerId");

                    b.Navigation("Player");

                    b.Navigation("Sport");

                    b.Navigation("Trainer");
                });

            modelBuilder.Entity("PlatinumGym.Core.Models.TrainingProgram.PlayerProgram", b =>
                {
                    b.HasOne("PlatinumGym.Core.Models.Sport.Sport", "Sport")
                        .WithMany()
                        .HasForeignKey("SportId");

                    b.Navigation("Sport");
                });

            modelBuilder.Entity("PlatinumGym.Core.Models.TrainingProgram.Training", b =>
                {
                    b.HasOne("PlatinumGym.Core.Models.TrainingProgram.TrainingCategory", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId");

                    b.HasOne("PlatinumGym.Core.Models.Sport.Sport", "Sport")
                        .WithMany()
                        .HasForeignKey("SportId");

                    b.Navigation("Category");

                    b.Navigation("Sport");
                });

            modelBuilder.Entity("PlatinumGym.Core.Models.TrainingProgram.TrainingProgram", b =>
                {
                    b.HasOne("PlatinumGym.Core.Models.TrainingProgram.TrainingCategory", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId");

                    b.HasOne("PlatinumGym.Core.Models.TrainingProgram.PlayerProgram", "PlayerProgram")
                        .WithMany()
                        .HasForeignKey("PlayerProgramId");

                    b.HasOne("PlatinumGym.Core.Models.TrainingProgram.Training", "Trainings")
                        .WithMany()
                        .HasForeignKey("TrainingsId");

                    b.Navigation("Category");

                    b.Navigation("PlayerProgram");

                    b.Navigation("Trainings");
                });

            modelBuilder.Entity("PlatinumGym.Core.Models.Sport.Sport", b =>
                {
                    b.Navigation("PlayerTrainings");
                });
#pragma warning restore 612, 618
        }
    }
}
