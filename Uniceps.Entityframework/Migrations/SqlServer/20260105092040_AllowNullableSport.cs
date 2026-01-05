using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Uniceps.Entityframework.Migrations.SqlServer
{
    /// <inheritdoc />
    public partial class AllowNullableSport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subscriptions_Sports_SportId",
                table: "Subscriptions");

            migrationBuilder.AlterColumn<int>(
                name: "SportId",
                table: "Subscriptions",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "SportId1",
                table: "Subscriptions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_SportId1",
                table: "Subscriptions",
                column: "SportId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Subscriptions_Sports_SportId",
                table: "Subscriptions",
                column: "SportId",
                principalTable: "Sports",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Subscriptions_Sports_SportId1",
                table: "Subscriptions",
                column: "SportId1",
                principalTable: "Sports",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subscriptions_Sports_SportId",
                table: "Subscriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscriptions_Sports_SportId1",
                table: "Subscriptions");

            migrationBuilder.DropIndex(
                name: "IX_Subscriptions_SportId1",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "SportId1",
                table: "Subscriptions");

            migrationBuilder.AlterColumn<int>(
                name: "SportId",
                table: "Subscriptions",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Subscriptions_Sports_SportId",
                table: "Subscriptions",
                column: "SportId",
                principalTable: "Sports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
