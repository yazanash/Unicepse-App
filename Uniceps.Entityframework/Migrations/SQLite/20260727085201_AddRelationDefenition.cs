using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Uniceps.Entityframework.Migrations.SQLite
{
    /// <inheritdoc />
    public partial class AddRelationDefenition : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExercisesV2_Equipment_EquipmentCode",
                table: "ExercisesV2");

            migrationBuilder.DropForeignKey(
                name: "FK_ExercisesV2_MuscleGroupsV2_MuscleGroupCode",
                table: "ExercisesV2");

            migrationBuilder.DropForeignKey(
                name: "FK_ExercisesV2_MuscleHeads_MuscleHeadCode",
                table: "ExercisesV2");

            migrationBuilder.DropForeignKey(
                name: "FK_MuscleHeads_MuscleGroupsV2_MuscleGroupCode",
                table: "MuscleHeads");

            migrationBuilder.AddColumn<string>(
                name: "MuscleGroupV2Code",
                table: "MuscleHeads",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MuscleHeads_MuscleGroupV2Code",
                table: "MuscleHeads",
                column: "MuscleGroupV2Code");

            migrationBuilder.AddForeignKey(
                name: "FK_ExercisesV2_Equipment_EquipmentCode",
                table: "ExercisesV2",
                column: "EquipmentCode",
                principalTable: "Equipment",
                principalColumn: "Code");

            migrationBuilder.AddForeignKey(
                name: "FK_ExercisesV2_MuscleGroupsV2_MuscleGroupCode",
                table: "ExercisesV2",
                column: "MuscleGroupCode",
                principalTable: "MuscleGroupsV2",
                principalColumn: "Code");

            migrationBuilder.AddForeignKey(
                name: "FK_ExercisesV2_MuscleHeads_MuscleHeadCode",
                table: "ExercisesV2",
                column: "MuscleHeadCode",
                principalTable: "MuscleHeads",
                principalColumn: "Code");

            migrationBuilder.AddForeignKey(
                name: "FK_MuscleHeads_MuscleGroupsV2_MuscleGroupCode",
                table: "MuscleHeads",
                column: "MuscleGroupCode",
                principalTable: "MuscleGroupsV2",
                principalColumn: "Code");

            migrationBuilder.AddForeignKey(
                name: "FK_MuscleHeads_MuscleGroupsV2_MuscleGroupV2Code",
                table: "MuscleHeads",
                column: "MuscleGroupV2Code",
                principalTable: "MuscleGroupsV2",
                principalColumn: "Code");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExercisesV2_Equipment_EquipmentCode",
                table: "ExercisesV2");

            migrationBuilder.DropForeignKey(
                name: "FK_ExercisesV2_MuscleGroupsV2_MuscleGroupCode",
                table: "ExercisesV2");

            migrationBuilder.DropForeignKey(
                name: "FK_ExercisesV2_MuscleHeads_MuscleHeadCode",
                table: "ExercisesV2");

            migrationBuilder.DropForeignKey(
                name: "FK_MuscleHeads_MuscleGroupsV2_MuscleGroupCode",
                table: "MuscleHeads");

            migrationBuilder.DropForeignKey(
                name: "FK_MuscleHeads_MuscleGroupsV2_MuscleGroupV2Code",
                table: "MuscleHeads");

            migrationBuilder.DropIndex(
                name: "IX_MuscleHeads_MuscleGroupV2Code",
                table: "MuscleHeads");

            migrationBuilder.DropColumn(
                name: "MuscleGroupV2Code",
                table: "MuscleHeads");

            migrationBuilder.AddForeignKey(
                name: "FK_ExercisesV2_Equipment_EquipmentCode",
                table: "ExercisesV2",
                column: "EquipmentCode",
                principalTable: "Equipment",
                principalColumn: "Code",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExercisesV2_MuscleGroupsV2_MuscleGroupCode",
                table: "ExercisesV2",
                column: "MuscleGroupCode",
                principalTable: "MuscleGroupsV2",
                principalColumn: "Code",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExercisesV2_MuscleHeads_MuscleHeadCode",
                table: "ExercisesV2",
                column: "MuscleHeadCode",
                principalTable: "MuscleHeads",
                principalColumn: "Code",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MuscleHeads_MuscleGroupsV2_MuscleGroupCode",
                table: "MuscleHeads",
                column: "MuscleGroupCode",
                principalTable: "MuscleGroupsV2",
                principalColumn: "Code",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
