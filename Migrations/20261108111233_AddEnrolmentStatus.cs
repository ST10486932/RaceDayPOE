using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RaceDay2.Migrations
{
    /// <inheritdoc />
    public partial class AddEnrolmentStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Enrolments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Enrolments");
        }
    }
}
