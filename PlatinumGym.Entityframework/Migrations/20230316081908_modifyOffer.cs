using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlatinumGymPro.Migrations
{
    /// <inheritdoc />
    public partial class modifyOffer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayerTrainings_Offer_OfferId",
                table: "PlayerTrainings");

            migrationBuilder.DropIndex(
                name: "IX_PlayerTrainings_OfferId",
                table: "PlayerTrainings");

            migrationBuilder.DropColumn(
                name: "OfferId",
                table: "PlayerTrainings");

            migrationBuilder.AddColumn<string>(
                name: "OfferDes",
                table: "PlayerTrainings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "OfferValue",
                table: "PlayerTrainings",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OfferDes",
                table: "PlayerTrainings");

            migrationBuilder.DropColumn(
                name: "OfferValue",
                table: "PlayerTrainings");

            migrationBuilder.AddColumn<int>(
                name: "OfferId",
                table: "PlayerTrainings",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlayerTrainings_OfferId",
                table: "PlayerTrainings",
                column: "OfferId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerTrainings_Offer_OfferId",
                table: "PlayerTrainings",
                column: "OfferId",
                principalTable: "Offer",
                principalColumn: "Id");
        }
    }
}
