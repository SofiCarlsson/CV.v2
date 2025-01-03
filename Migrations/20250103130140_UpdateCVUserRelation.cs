using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CV_v2.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCVUserRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_CVs_CVID",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_CVID",
                table: "Users");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "CVs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "CVs",
                keyColumn: "CVId",
                keyValue: 1,
                column: "UserId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "CVs",
                keyColumn: "CVId",
                keyValue: 2,
                column: "UserId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "CVs",
                keyColumn: "CVId",
                keyValue: 3,
                column: "UserId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "CVs",
                keyColumn: "CVId",
                keyValue: 4,
                column: "UserId",
                value: 4);

            migrationBuilder.CreateIndex(
                name: "IX_Users_CVID",
                table: "Users",
                column: "CVID",
                unique: true,
                filter: "[CVID] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_CVs_CVID",
                table: "Users",
                column: "CVID",
                principalTable: "CVs",
                principalColumn: "CVId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_CVs_CVID",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_CVID",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "CVs");

            migrationBuilder.CreateIndex(
                name: "IX_Users_CVID",
                table: "Users",
                column: "CVID");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_CVs_CVID",
                table: "Users",
                column: "CVID",
                principalTable: "CVs",
                principalColumn: "CVId");
        }
    }
}
