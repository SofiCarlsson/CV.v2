using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CV_v2.Migrations
{
    /// <inheritdoc />
    public partial class CompetencesHej : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Competences",
                table: "CVs",
                newName: "CompetencesHej");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CompetencesHej",
                table: "CVs",
                newName: "Competences");
        }
    }
}
