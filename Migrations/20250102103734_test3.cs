using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CV_v2.Migrations
{
    /// <inheritdoc />
    public partial class test3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "CVs",
                columns: new[] { "CVId", "Competences", "Education", "WorkExperience" },
                values: new object[,]
                {
                    { 1, "SQL", "Örebro Universitet", "Tränare" },
                    { 2, "C#", "Örebro Universitet", "Arla Foods" },
                    { 3, "CSS, HTML", "Örebro Universitet", "PostNord" },
                    { 4, "Java", "Örebro Universitet", "IKEA" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "CVID", "Email", "Firstname", "Lastname", "Password" },
                values: new object[,]
                {
                    { 1, 1, "clara.lunak04@gmail.com", "Clara", "Lunak", "svampnisse" },
                    { 2, 2, "carlssonsofi99@gmail.com", "Sofi", "Carlsson", "juan123" },
                    { 3, 3, "olivia.cleve@gmail.com", "Olivia", "Cleve", "olivia123" },
                    { 4, 4, "malin.sandberg@gmail.com", "Malin", "Sandberg", "malin123" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "CVs",
                keyColumn: "CVId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "CVs",
                keyColumn: "CVId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "CVs",
                keyColumn: "CVId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "CVs",
                keyColumn: "CVId",
                keyValue: 4);
        }
    }
}
