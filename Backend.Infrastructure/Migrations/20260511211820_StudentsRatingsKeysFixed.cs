using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class StudentsRatingsKeysFixed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentRatings_Subjects_SubjectId",
                table: "StudentRatings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentRatings",
                table: "StudentRatings");

            migrationBuilder.AlterColumn<Guid>(
                name: "SubjectId",
                table: "StudentRatings",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentRatings",
                table: "StudentRatings",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_StudentRatings_StudentId",
                table: "StudentRatings",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentRatings_Subjects_SubjectId",
                table: "StudentRatings",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentRatings_Subjects_SubjectId",
                table: "StudentRatings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentRatings",
                table: "StudentRatings");

            migrationBuilder.DropIndex(
                name: "IX_StudentRatings_StudentId",
                table: "StudentRatings");

            migrationBuilder.AlterColumn<Guid>(
                name: "SubjectId",
                table: "StudentRatings",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentRatings",
                table: "StudentRatings",
                columns: new[] { "StudentId", "SubjectId" });

            migrationBuilder.AddForeignKey(
                name: "FK_StudentRatings_Subjects_SubjectId",
                table: "StudentRatings",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
