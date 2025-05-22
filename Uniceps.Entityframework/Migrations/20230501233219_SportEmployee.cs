using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Unicepse.Entityframework.Migrations
{
    /// <inheritdoc />
    public partial class SportEmployee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Credit_Employee_EmpPersonId",
                table: "Credit");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeSport_Employee_TrainersId",
                table: "EmployeeSport");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeSport_Sport_SportsId",
                table: "EmployeeSport");

            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_Employee_RecipientId",
                table: "Expenses");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerPayments_Employee_RecipientId",
                table: "PlayerPayments");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerProgram_Sport_SportId",
                table: "PlayerProgram");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerTrainings_Employee_TrainerId",
                table: "PlayerTrainings");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerTrainings_Sport_SportId",
                table: "PlayerTrainings");

            migrationBuilder.DropForeignKey(
                name: "FK_TrainerDueses_Employee_TrainerId",
                table: "TrainerDueses");

            migrationBuilder.DropForeignKey(
                name: "FK_Training_Sport_SportId",
                table: "Training");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Employee_EmployeeId",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Sport",
                table: "Sport");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Employee",
                table: "Employee");

            migrationBuilder.RenameTable(
                name: "Sport",
                newName: "Sports");

            migrationBuilder.RenameTable(
                name: "Employee",
                newName: "Employees");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Sports",
                table: "Sports",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Employees",
                table: "Employees",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Credit_Employees_EmpPersonId",
                table: "Credit",
                column: "EmpPersonId",
                principalTable: "Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeSport_Employees_TrainersId",
                table: "EmployeeSport",
                column: "TrainersId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeSport_Sports_SportsId",
                table: "EmployeeSport",
                column: "SportsId",
                principalTable: "Sports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_Employees_RecipientId",
                table: "Expenses",
                column: "RecipientId",
                principalTable: "Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerPayments_Employees_RecipientId",
                table: "PlayerPayments",
                column: "RecipientId",
                principalTable: "Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerProgram_Sports_SportId",
                table: "PlayerProgram",
                column: "SportId",
                principalTable: "Sports",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerTrainings_Employees_TrainerId",
                table: "PlayerTrainings",
                column: "TrainerId",
                principalTable: "Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerTrainings_Sports_SportId",
                table: "PlayerTrainings",
                column: "SportId",
                principalTable: "Sports",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TrainerDueses_Employees_TrainerId",
                table: "TrainerDueses",
                column: "TrainerId",
                principalTable: "Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Training_Sports_SportId",
                table: "Training",
                column: "SportId",
                principalTable: "Sports",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Employees_EmployeeId",
                table: "Users",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Credit_Employees_EmpPersonId",
                table: "Credit");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeSport_Employees_TrainersId",
                table: "EmployeeSport");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeSport_Sports_SportsId",
                table: "EmployeeSport");

            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_Employees_RecipientId",
                table: "Expenses");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerPayments_Employees_RecipientId",
                table: "PlayerPayments");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerProgram_Sports_SportId",
                table: "PlayerProgram");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerTrainings_Employees_TrainerId",
                table: "PlayerTrainings");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerTrainings_Sports_SportId",
                table: "PlayerTrainings");

            migrationBuilder.DropForeignKey(
                name: "FK_TrainerDueses_Employees_TrainerId",
                table: "TrainerDueses");

            migrationBuilder.DropForeignKey(
                name: "FK_Training_Sports_SportId",
                table: "Training");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Employees_EmployeeId",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Sports",
                table: "Sports");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Employees",
                table: "Employees");

            migrationBuilder.RenameTable(
                name: "Sports",
                newName: "Sport");

            migrationBuilder.RenameTable(
                name: "Employees",
                newName: "Employee");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Sport",
                table: "Sport",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Employee",
                table: "Employee",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Credit_Employee_EmpPersonId",
                table: "Credit",
                column: "EmpPersonId",
                principalTable: "Employee",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeSport_Employee_TrainersId",
                table: "EmployeeSport",
                column: "TrainersId",
                principalTable: "Employee",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeSport_Sport_SportsId",
                table: "EmployeeSport",
                column: "SportsId",
                principalTable: "Sport",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_Employee_RecipientId",
                table: "Expenses",
                column: "RecipientId",
                principalTable: "Employee",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerPayments_Employee_RecipientId",
                table: "PlayerPayments",
                column: "RecipientId",
                principalTable: "Employee",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerProgram_Sport_SportId",
                table: "PlayerProgram",
                column: "SportId",
                principalTable: "Sport",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerTrainings_Employee_TrainerId",
                table: "PlayerTrainings",
                column: "TrainerId",
                principalTable: "Employee",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerTrainings_Sport_SportId",
                table: "PlayerTrainings",
                column: "SportId",
                principalTable: "Sport",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TrainerDueses_Employee_TrainerId",
                table: "TrainerDueses",
                column: "TrainerId",
                principalTable: "Employee",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Training_Sport_SportId",
                table: "Training",
                column: "SportId",
                principalTable: "Sport",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Employee_EmployeeId",
                table: "Users",
                column: "EmployeeId",
                principalTable: "Employee",
                principalColumn: "Id");
        }
    }
}
