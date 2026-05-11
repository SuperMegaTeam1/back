using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class StudentsRatingsChanged : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Rating",
                table: "StudentRatings",
                newName: "TotalGrade");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "StudentRatings",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "GroupId",
                table: "StudentRatings",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "StudentRatings",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "RatingPosition",
                table: "StudentRatings",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "StudentRatings",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "StudentRatings");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "StudentRatings");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "StudentRatings");

            migrationBuilder.DropColumn(
                name: "RatingPosition",
                table: "StudentRatings");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "StudentRatings");

            migrationBuilder.RenameColumn(
                name: "TotalGrade",
                table: "StudentRatings",
                newName: "Rating");
        }
    }
}
