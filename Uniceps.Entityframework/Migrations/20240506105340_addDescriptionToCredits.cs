using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Unicepse.Entityframework.Migrations
{
    /// <inheritdoc />
    public partial class addDescriptionToCredits : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrainerDueses_PlayerPayments_PlayerPaymentId",
                table: "TrainerDueses");

            migrationBuilder.DropForeignKey(
                name: "FK_TrainerDueses_Subscriptions_PlayerTrainingId",
                table: "TrainerDueses");

            migrationBuilder.DropIndex(
                name: "IX_TrainerDueses_PlayerPaymentId",
                table: "TrainerDueses");

            migrationBuilder.DropIndex(
                name: "IX_TrainerDueses_PlayerTrainingId",
                table: "TrainerDueses");

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 46);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 47);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 48);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 49);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 50);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 51);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 52);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 53);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 54);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 55);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 56);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 57);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 58);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 59);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 60);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 61);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 62);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 63);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 64);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 65);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 66);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 67);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 68);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 69);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 70);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 71);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 72);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 73);

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "TrainerDueses");

            migrationBuilder.DropColumn(
                name: "From",
                table: "TrainerDueses");

            migrationBuilder.DropColumn(
                name: "IsPrivate",
                table: "TrainerDueses");

            migrationBuilder.DropColumn(
                name: "PlayerPaymentId",
                table: "TrainerDueses");

            migrationBuilder.DropColumn(
                name: "PlayerTrainingId",
                table: "TrainerDueses");

            migrationBuilder.RenameColumn(
                name: "Value",
                table: "TrainerDueses",
                newName: "TotalSubscriptions");

            migrationBuilder.RenameColumn(
                name: "To",
                table: "TrainerDueses",
                newName: "IssueDate");

            migrationBuilder.AddColumn<int>(
                name: "CountSubscription",
                table: "TrainerDueses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "Credits",
                table: "TrainerDueses",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "CreditsCount",
                table: "TrainerDueses",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Parcent",
                table: "TrainerDueses",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Salary",
                table: "TrainerDueses",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AlterColumn<string>(
                name: "DaysGroupMap",
                table: "PlayerRoutine",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Credit",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CountSubscription",
                table: "TrainerDueses");

            migrationBuilder.DropColumn(
                name: "Credits",
                table: "TrainerDueses");

            migrationBuilder.DropColumn(
                name: "CreditsCount",
                table: "TrainerDueses");

            migrationBuilder.DropColumn(
                name: "Parcent",
                table: "TrainerDueses");

            migrationBuilder.DropColumn(
                name: "Salary",
                table: "TrainerDueses");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Credit");

            migrationBuilder.RenameColumn(
                name: "TotalSubscriptions",
                table: "TrainerDueses",
                newName: "Value");

            migrationBuilder.RenameColumn(
                name: "IssueDate",
                table: "TrainerDueses",
                newName: "To");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "TrainerDueses",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "From",
                table: "TrainerDueses",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsPrivate",
                table: "TrainerDueses",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "PlayerPaymentId",
                table: "TrainerDueses",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PlayerTrainingId",
                table: "TrainerDueses",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DaysGroupMap",
                table: "PlayerRoutine",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.InsertData(
                table: "Exercises",
                columns: new[] { "Id", "GroupId", "ImageId", "Muscel", "Name", "Tid" },
                values: new object[,]
                {
                    { 1, 1, "6", "صدر", "ضغط مستوي بالبار", 1 },
                    { 2, 1, "8", "صدر", "ضغط عالي بالبار", 2 },
                    { 3, 1, "2", "صدر", "ضغط اسفل بالبار", 3 },
                    { 4, 1, null, "صدر", "ضغط جالسا على الالة ", 4 },
                    { 5, 1, "1", "صدر", "ضغط عالي على الألة", 5 },
                    { 6, 1, "9", "صدر", "متوازي", 6 },
                    { 7, 1, "3", "صدر", "ضغط دامبلز", 7 },
                    { 8, 1, null, "صدر", "فتح دامبلز", 8 },
                    { 9, 1, "5", "صدر", "تجميع بالالة", 9 },
                    { 10, 1, "7", "صدر", "تجميع بالكابل ", 10 },
                    { 11, 1, "4", "صدر", "بلوفر", 11 },
                    { 12, 2, "2", "اكتاف", "ضغط بار خلف الراس جالسا", 1 },
                    { 13, 2, "8", "اكتاف", "ضغط بار امام الراس جالسا", 2 },
                    { 14, 2, "1", "اكتاف", "ضغط دامبلز جالسا", 3 },
                    { 15, 2, "10", "اكتاف", "تبادل امامي دامبلز", 4 },
                    { 16, 2, "6", "اكتاف", "رفع امامي يالبار مستلقيا على الكرسي المائل", 5 },
                    { 17, 2, "3", "اكتاف", "رفرفه جانبية بالدامبلز واقفا ", 6 },
                    { 18, 2, "11", "اكتاف", "رفع جانبي بالدامبلز مستلقيا", 7 },
                    { 19, 2, "4", "اكتاف", "رفع جانبي بالكابل ", 8 },
                    { 20, 2, "5", "اكتاف", "رفع جانبي منحني 90", 9 },
                    { 21, 2, "9", "اكتاف", "نشل بالبار تحت الذقن ", 10 },
                    { 22, 2, "7", "اكتاف", "هز الاكتاف ", 11 },
                    { 23, 3, "3", "ظهر", "السحب بالبكرة ", 1 },
                    { 24, 3, "2", "ظهر", "السحب على الالة للاسفل", 2 },
                    { 25, 3, "9", "ظهر", "رفع الجذع على الكرسي الروماني", 3 },
                    { 26, 3, "7", "ظهر", "الرفعة الميتة", 4 },
                    { 27, 3, "4", "ظهر", "السحب على الثابت", 5 },
                    { 28, 3, null, "ظهر", "السحب العلوي بالقبضات المتقابلة", 6 },
                    { 29, 3, "8", "ظهر", "التجديف جالسا بالقبضات المتقابلة", 7 },
                    { 30, 3, "6", "ظهر", "التجديف بالدامبلز", 8 },
                    { 31, 3, "1", "ظهر", "T - BAR", 9 },
                    { 32, 3, "10", "ظهر", "تجديف منحني بالبار ", 10 },
                    { 33, 3, null, "ظهر", "تجديف جالسا", 11 },
                    { 34, 4, "5", "ارجل", "سكوات", 1 },
                    { 35, 4, "3", "ارجل", "مكبس", 2 },
                    { 36, 4, "10", "ارجل", "هاك سكوات", 3 },
                    { 37, 4, "4", "ارجل", "طاولة امامي", 4 },
                    { 38, 4, "9", "ارجل", "سيسي سكوات", 5 },
                    { 39, 4, "7", "ارجل", "الاندفاع للامام", 6 },
                    { 40, 4, "8", "ارجل", "رفع الارجل شبه مشدودة للاعلى", 7 },
                    { 41, 4, "1", "ارجل", "ثني الارجل مستلقيا", 8 },
                    { 42, 4, "2", "ارجل", "الثني برجل واحدة ", 9 },
                    { 43, 4, "6", "ارجل", "الرفعة الميتة", 10 },
                    { 44, 5, "1", "بايسيبس", "واقفا بالبار", 1 },
                    { 45, 5, "6", "بايسيبس", "مقعد سكوات", 2 },
                    { 46, 5, "4", "بايسيبس", "ثني اليدين بالبكرة", 3 },
                    { 47, 5, "8", "بايسيبس", "تبادل بالدامبلز", 4 },
                    { 48, 5, "5", "بايسيبس", "تركيز بالدامبلز", 5 },
                    { 49, 5, "3", "بايسيبس", "تركيز بالكابل", 6 },
                    { 50, 5, "7", "بايسيبس", "مقعد سكوت بالدامبلز", 7 },
                    { 51, 5, "2", "بايسيبس", "هز سواعد بالبار ", 8 },
                    { 52, 6, "1", "تراي سيبس", "ثني اليدين بالبار مستلقيا ", 1 },
                    { 53, 6, "8", "تراي سيبس", "ثني اليدين جالسا بالبار", 2 },
                    { 54, 6, "3", "تراي سيبس", "ضغط مستوي قبضة ضيقة ", 3 },
                    { 55, 6, null, "تراي سيبس", "الغطس بين مقعدين", 4 },
                    { 56, 6, "7", "تراي سيبس", "الدفع للاسفل بالبكرة ", 5 },
                    { 57, 6, "4", "تراي سيبس", "سحب خلف الراس بالكابل", 6 },
                    { 58, 6, "6", "تراي سيبس", "تركيز دامبل خلف الراس", 7 },
                    { 59, 6, "5", "تراي سيبس", "تركيز بالدامبل مستلقيا", 8 },
                    { 60, 6, "9", "تراي سيبس", "متوازي", 9 },
                    { 61, 7, "5", "بطات الارجل", "واقفا ", 1 },
                    { 62, 7, "1", "بطات الارجل", "مكبس", 2 },
                    { 63, 7, "2", "بطات الارجل", "هاك سكواد", 3 },
                    { 64, 7, "4", "بطات الارجل", "واقفا برجل واحدة", 4 },
                    { 65, 7, "3", "بطات الارجل", "جالسا", 5 },
                    { 66, 8, null, "البطن", "رفع الجذع مستلقيا", 1 },
                    { 67, 8, "6", "البطن", "مد و جزر", 2 },
                    { 68, 8, "1", "البطن", "طحن", 3 },
                    { 69, 8, "8", "البطن", "طحن بالكابل", 4 },
                    { 70, 8, "7", "البطن", "رفع الارجل مستلقيا", 5 },
                    { 71, 8, "3", "البطن", "رفع الارجل على المتوازي", 6 },
                    { 72, 8, "5", "البطن", "رفع الارجل على الثابت", 7 },
                    { 73, 8, "4", "البطن", "دوران الجذع جالسا ", 8 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_TrainerDueses_PlayerPaymentId",
                table: "TrainerDueses",
                column: "PlayerPaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainerDueses_PlayerTrainingId",
                table: "TrainerDueses",
                column: "PlayerTrainingId");

            migrationBuilder.AddForeignKey(
                name: "FK_TrainerDueses_PlayerPayments_PlayerPaymentId",
                table: "TrainerDueses",
                column: "PlayerPaymentId",
                principalTable: "PlayerPayments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TrainerDueses_Subscriptions_PlayerTrainingId",
                table: "TrainerDueses",
                column: "PlayerTrainingId",
                principalTable: "Subscriptions",
                principalColumn: "Id");
        }
    }
}
