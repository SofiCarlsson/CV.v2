using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CV_v2.Migrations
{
    /// <inheritdoc />
    public partial class upd_cvClss : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PicturePath",
                table: "CVs",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PicturePath",
                table: "CVs");
        }
    }
}
