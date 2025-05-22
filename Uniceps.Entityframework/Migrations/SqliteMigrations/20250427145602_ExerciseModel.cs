using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Unicepse.Entityframework.Migrations.SqliteMigrations
{
    /// <inheritdoc />
    public partial class ExerciseModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageId",
                table: "Exercises",
                newName: "ImagePath");

            migrationBuilder.RenameColumn(
                name: "GroupId",
                table: "Exercises",
                newName: "Version");

            migrationBuilder.AddColumn<int>(
                name: "MuscleGroupId",
                table: "Exercises",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MuscleGroupId",
                table: "Exercises");

            migrationBuilder.RenameColumn(
                name: "Version",
                table: "Exercises",
                newName: "GroupId");

            migrationBuilder.RenameColumn(
                name: "ImagePath",
                table: "Exercises",
                newName: "ImageId");
        }
    }
}
