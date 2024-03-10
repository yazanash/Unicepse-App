using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlatinumGymPro.Migrations
{
    /// <inheritdoc />
    public partial class removeroutineItemsToJs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoutineItems_PlayerRoutine_PlayerRoutineId",
                table: "RoutineItems");

            migrationBuilder.DropIndex(
                name: "IX_RoutineItems_PlayerRoutineId",
                table: "RoutineItems");

            migrationBuilder.DropColumn(
                name: "PlayerRoutineId",
                table: "RoutineItems");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PlayerRoutineId",
                table: "RoutineItems",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RoutineItems_PlayerRoutineId",
                table: "RoutineItems",
                column: "PlayerRoutineId");

            migrationBuilder.AddForeignKey(
                name: "FK_RoutineItems_PlayerRoutine_PlayerRoutineId",
                table: "RoutineItems",
                column: "PlayerRoutineId",
                principalTable: "PlayerRoutine",
                principalColumn: "Id");
        }
    }
}
