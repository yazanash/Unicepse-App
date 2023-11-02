using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlatinumGymPro.Migrations
{
    /// <inheritdoc />
    public partial class SportTrainers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employee_Sport_SportId",
                table: "Employee");

            migrationBuilder.DropIndex(
                name: "IX_Employee_SportId",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "SportId",
                table: "Employee");

            migrationBuilder.CreateTable(
                name: "EmployeeSport",
                columns: table => new
                {
                    SportsId = table.Column<int>(type: "int", nullable: false),
                    TrainersId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeSport", x => new { x.SportsId, x.TrainersId });
                    table.ForeignKey(
                        name: "FK_EmployeeSport_Employee_TrainersId",
                        column: x => x.TrainersId,
                        principalTable: "Employee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeSport_Sport_SportsId",
                        column: x => x.SportsId,
                        principalTable: "Sport",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeSport_TrainersId",
                table: "EmployeeSport",
                column: "TrainersId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeSport");

            migrationBuilder.AddColumn<int>(
                name: "SportId",
                table: "Employee",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employee_SportId",
                table: "Employee",
                column: "SportId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_Sport_SportId",
                table: "Employee",
                column: "SportId",
                principalTable: "Sport",
                principalColumn: "Id");
        }
    }
}
