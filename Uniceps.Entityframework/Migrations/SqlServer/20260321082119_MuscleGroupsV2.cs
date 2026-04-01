using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Uniceps.Entityframework.Migrations.SqlServer
{
    /// <inheritdoc />
    public partial class MuscleGroupsV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "MuscleHeadCode",
                table: "ExercisesV2",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "MuscleGroupCode",
                table: "ExercisesV2",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "EquipmentCode",
                table: "ExercisesV2",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "Equipment",
                columns: table => new
                {
                    Code = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equipment", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "MuscleGroupsV2",
                columns: table => new
                {
                    Code = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MuscleGroupsV2", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "MuscleHeads",
                columns: table => new
                {
                    Code = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MuscleGroupCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MuscleHeads", x => x.Code);
                    table.ForeignKey(
                        name: "FK_MuscleHeads_MuscleGroupsV2_MuscleGroupCode",
                        column: x => x.MuscleGroupCode,
                        principalTable: "MuscleGroupsV2",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExercisesV2_EquipmentCode",
                table: "ExercisesV2",
                column: "EquipmentCode");

            migrationBuilder.CreateIndex(
                name: "IX_ExercisesV2_MuscleGroupCode",
                table: "ExercisesV2",
                column: "MuscleGroupCode");

            migrationBuilder.CreateIndex(
                name: "IX_ExercisesV2_MuscleHeadCode",
                table: "ExercisesV2",
                column: "MuscleHeadCode");

            migrationBuilder.CreateIndex(
                name: "IX_MuscleHeads_MuscleGroupCode",
                table: "MuscleHeads",
                column: "MuscleGroupCode");

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

            migrationBuilder.DropTable(
                name: "Equipment");

            migrationBuilder.DropTable(
                name: "MuscleHeads");

            migrationBuilder.DropTable(
                name: "MuscleGroupsV2");

            migrationBuilder.DropIndex(
                name: "IX_ExercisesV2_EquipmentCode",
                table: "ExercisesV2");

            migrationBuilder.DropIndex(
                name: "IX_ExercisesV2_MuscleGroupCode",
                table: "ExercisesV2");

            migrationBuilder.DropIndex(
                name: "IX_ExercisesV2_MuscleHeadCode",
                table: "ExercisesV2");

            migrationBuilder.AlterColumn<string>(
                name: "MuscleHeadCode",
                table: "ExercisesV2",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "MuscleGroupCode",
                table: "ExercisesV2",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "EquipmentCode",
                table: "ExercisesV2",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
