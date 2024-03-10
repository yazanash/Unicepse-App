using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlatinumGymPro.Migrations
{
    /// <inheritdoc />
    public partial class routineItemsToJsonString : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoutineItems");

            migrationBuilder.AddColumn<string>(
                name: "RoutineSchedule",
                table: "PlayerRoutine",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RoutineSchedule",
                table: "PlayerRoutine");

            migrationBuilder.CreateTable(
                name: "RoutineItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExerciseId = table.Column<int>(type: "int", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(4000)", nullable: true),
                    Orders = table.Column<string>(type: "nvarchar(4000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoutineItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoutineItems_Exercises_ExerciseId",
                        column: x => x.ExerciseId,
                        principalTable: "Exercises",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_RoutineItems_ExerciseId",
                table: "RoutineItems",
                column: "ExerciseId");
        }
    }
}
