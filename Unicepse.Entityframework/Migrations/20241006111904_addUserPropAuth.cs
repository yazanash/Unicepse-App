using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Unicepse.Entityframework.Migrations
{
    /// <inheritdoc />
    public partial class addUserPropAuth : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "authenticationLogs",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_authenticationLogs_UserId",
                table: "authenticationLogs",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_authenticationLogs_Users_UserId",
                table: "authenticationLogs",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_authenticationLogs_Users_UserId",
                table: "authenticationLogs");

            migrationBuilder.DropIndex(
                name: "IX_authenticationLogs_UserId",
                table: "authenticationLogs");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "authenticationLogs",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
