using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveStudyGroupTimestamps : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "StudyGroups");
            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "StudyGroups");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "StudyGroups",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime());
            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "StudyGroups",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime());
        }
    }
}
