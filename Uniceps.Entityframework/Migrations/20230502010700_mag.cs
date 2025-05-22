using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Unicepse.Entityframework.Migrations
{
    /// <inheritdoc />
    public partial class mag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SportsEmployee",
                table: "SportsEmployee");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "SportsEmployee",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SportsEmployee",
                table: "SportsEmployee",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_SportsEmployee_SportId",
                table: "SportsEmployee",
                column: "SportId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SportsEmployee",
                table: "SportsEmployee");

            migrationBuilder.DropIndex(
                name: "IX_SportsEmployee_SportId",
                table: "SportsEmployee");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "SportsEmployee");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SportsEmployee",
                table: "SportsEmployee",
                columns: new[] { "SportId", "EmployeeId" });
        }
    }
}
