using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixStudentGroupRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_StudyGroups_StudyGroupId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "Students");

            migrationBuilder.AlterColumn<Guid>(
                name: "StudyGroupId",
                table: "Students",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Students_StudyGroups_StudyGroupId",
                table: "Students",
                column: "StudyGroupId",
                principalTable: "StudyGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_StudyGroups_StudyGroupId",
                table: "Students");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "StudyGroups",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "StudyGroups",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<Guid>(
                name: "StudyGroupId",
                table: "Students",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<Guid>(
                name: "GroupId",
                table: "Students",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Students_StudyGroups_StudyGroupId",
                table: "Students",
                column: "StudyGroupId",
                principalTable: "StudyGroups",
                principalColumn: "Id");
        }
    }
}
