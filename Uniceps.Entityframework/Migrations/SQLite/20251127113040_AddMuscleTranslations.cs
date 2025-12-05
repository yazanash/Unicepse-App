using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Uniceps.Entityframework.Migrations.SQLite
{
    /// <inheritdoc />
    public partial class AddMuscleTranslations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Muscel",
                table: "Exercises",
                newName: "MuscelEng");

            migrationBuilder.AddColumn<string>(
                name: "MuscelAr",
                table: "Exercises",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MuscelAr",
                table: "Exercises");

            migrationBuilder.RenameColumn(
                name: "MuscelEng",
                table: "Exercises",
                newName: "Muscel");
        }
    }
}
