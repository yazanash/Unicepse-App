using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlatinumGymPro.Migrations
{
    /// <inheritdoc />
    public partial class modifySport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employee_Sport_Sportid",
                table: "Employee");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerProgram_Sport_Sportid",
                table: "PlayerProgram");

            migrationBuilder.DropForeignKey(
                name: "FK_Players_Sport_Sportid",
                table: "Players");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerTrainings_Sport_Sportid",
                table: "PlayerTrainings");

            migrationBuilder.DropForeignKey(
                name: "FK_Training_Sport_Sportid",
                table: "Training");

            migrationBuilder.DropIndex(
                name: "IX_Players_Sportid",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "Sportid",
                table: "Players");

            migrationBuilder.RenameColumn(
                name: "Sportid",
                table: "Training",
                newName: "SportId");

            migrationBuilder.RenameIndex(
                name: "IX_Training_Sportid",
                table: "Training",
                newName: "IX_Training_SportId");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Sport",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "Sportid",
                table: "PlayerTrainings",
                newName: "SportId");

            migrationBuilder.RenameIndex(
                name: "IX_PlayerTrainings_Sportid",
                table: "PlayerTrainings",
                newName: "IX_PlayerTrainings_SportId");

            migrationBuilder.RenameColumn(
                name: "Sportid",
                table: "PlayerProgram",
                newName: "SportId");

            migrationBuilder.RenameIndex(
                name: "IX_PlayerProgram_Sportid",
                table: "PlayerProgram",
                newName: "IX_PlayerProgram_SportId");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "PayReferance",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Expenses",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "Sportid",
                table: "Employee",
                newName: "SportId");

            migrationBuilder.RenameIndex(
                name: "IX_Employee_Sportid",
                table: "Employee",
                newName: "IX_Employee_SportId");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "DailyPlayerReport",
                newName: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_Sport_SportId",
                table: "Employee",
                column: "SportId",
                principalTable: "Sport",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerProgram_Sport_SportId",
                table: "PlayerProgram",
                column: "SportId",
                principalTable: "Sport",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerTrainings_Sport_SportId",
                table: "PlayerTrainings",
                column: "SportId",
                principalTable: "Sport",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Training_Sport_SportId",
                table: "Training",
                column: "SportId",
                principalTable: "Sport",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employee_Sport_SportId",
                table: "Employee");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerProgram_Sport_SportId",
                table: "PlayerProgram");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerTrainings_Sport_SportId",
                table: "PlayerTrainings");

            migrationBuilder.DropForeignKey(
                name: "FK_Training_Sport_SportId",
                table: "Training");

            migrationBuilder.RenameColumn(
                name: "SportId",
                table: "Training",
                newName: "Sportid");

            migrationBuilder.RenameIndex(
                name: "IX_Training_SportId",
                table: "Training",
                newName: "IX_Training_Sportid");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Sport",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "SportId",
                table: "PlayerTrainings",
                newName: "Sportid");

            migrationBuilder.RenameIndex(
                name: "IX_PlayerTrainings_SportId",
                table: "PlayerTrainings",
                newName: "IX_PlayerTrainings_Sportid");

            migrationBuilder.RenameColumn(
                name: "SportId",
                table: "PlayerProgram",
                newName: "Sportid");

            migrationBuilder.RenameIndex(
                name: "IX_PlayerProgram_SportId",
                table: "PlayerProgram",
                newName: "IX_PlayerProgram_Sportid");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "PayReferance",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Expenses",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "SportId",
                table: "Employee",
                newName: "Sportid");

            migrationBuilder.RenameIndex(
                name: "IX_Employee_SportId",
                table: "Employee",
                newName: "IX_Employee_Sportid");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "DailyPlayerReport",
                newName: "id");

            migrationBuilder.AddColumn<int>(
                name: "Sportid",
                table: "Players",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Players_Sportid",
                table: "Players",
                column: "Sportid");

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_Sport_Sportid",
                table: "Employee",
                column: "Sportid",
                principalTable: "Sport",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerProgram_Sport_Sportid",
                table: "PlayerProgram",
                column: "Sportid",
                principalTable: "Sport",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Players_Sport_Sportid",
                table: "Players",
                column: "Sportid",
                principalTable: "Sport",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerTrainings_Sport_Sportid",
                table: "PlayerTrainings",
                column: "Sportid",
                principalTable: "Sport",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Training_Sport_Sportid",
                table: "Training",
                column: "Sportid",
                principalTable: "Sport",
                principalColumn: "id");
        }
    }
}
