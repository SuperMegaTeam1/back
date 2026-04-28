using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class EntitiesRelationsAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "StudyGroupId",
                table: "Students",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "StudyGroupId",
                table: "Lessons",
                type: "uuid",
                nullable: true);

            migrationBuilder.Sql(
                """
                UPDATE "Lessons"
                SET "StudyGroupId" = "GroupId";
                """);

            migrationBuilder.Sql(
                """
                UPDATE "Students"
                SET "StudyGroupId" = "GroupId"
                WHERE "GroupId" IS NOT NULL;
                """);

            migrationBuilder.AlterColumn<Guid>(
                name: "StudyGroupId",
                table: "Lessons",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "LessonParticipations",
                columns: table => new
                {
                    StudentId = table.Column<Guid>(type: "uuid", nullable: false),
                    LessonId = table.Column<Guid>(type: "uuid", nullable: false),
                    Attended = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LessonParticipations", x => new { x.StudentId, x.LessonId });
                    table.ForeignKey(
                        name: "FK_LessonParticipations_Lessons_LessonId",
                        column: x => x.LessonId,
                        principalTable: "Lessons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LessonParticipations_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudentGrades",
                columns: table => new
                {
                    StudentId = table.Column<Guid>(type: "uuid", nullable: false),
                    LessonId = table.Column<Guid>(type: "uuid", nullable: false),
                    Grade = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentGrades", x => new { x.StudentId, x.LessonId });
                    table.ForeignKey(
                        name: "FK_StudentGrades_Lessons_LessonId",
                        column: x => x.LessonId,
                        principalTable: "Lessons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentGrades_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudentRatings",
                columns: table => new
                {
                    StudentId = table.Column<Guid>(type: "uuid", nullable: false),
                    SubjectId = table.Column<Guid>(type: "uuid", nullable: false),
                    Rating = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentRatings", x => new { x.StudentId, x.SubjectId });
                    table.ForeignKey(
                        name: "FK_StudentRatings_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentRatings_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_TeacherId",
                table: "Subjects",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_StudyGroupId",
                table: "Students",
                column: "StudyGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_StudyGroupId",
                table: "Lessons",
                column: "StudyGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_SubjectId",
                table: "Lessons",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_TeacherId",
                table: "Lessons",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_LessonParticipations_LessonId",
                table: "LessonParticipations",
                column: "LessonId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentGrades_LessonId",
                table: "StudentGrades",
                column: "LessonId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentRatings_SubjectId",
                table: "StudentRatings",
                column: "SubjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Lessons_StudyGroups_StudyGroupId",
                table: "Lessons",
                column: "StudyGroupId",
                principalTable: "StudyGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Lessons_Subjects_SubjectId",
                table: "Lessons",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Lessons_Teachers_TeacherId",
                table: "Lessons",
                column: "TeacherId",
                principalTable: "Teachers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Students_StudyGroups_StudyGroupId",
                table: "Students",
                column: "StudyGroupId",
                principalTable: "StudyGroups",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Subjects_Teachers_TeacherId",
                table: "Subjects",
                column: "TeacherId",
                principalTable: "Teachers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lessons_StudyGroups_StudyGroupId",
                table: "Lessons");

            migrationBuilder.DropForeignKey(
                name: "FK_Lessons_Subjects_SubjectId",
                table: "Lessons");

            migrationBuilder.DropForeignKey(
                name: "FK_Lessons_Teachers_TeacherId",
                table: "Lessons");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_StudyGroups_StudyGroupId",
                table: "Students");

            migrationBuilder.DropForeignKey(
                name: "FK_Subjects_Teachers_TeacherId",
                table: "Subjects");

            migrationBuilder.DropTable(
                name: "LessonParticipations");

            migrationBuilder.DropTable(
                name: "StudentGrades");

            migrationBuilder.DropTable(
                name: "StudentRatings");

            migrationBuilder.DropIndex(
                name: "IX_Subjects_TeacherId",
                table: "Subjects");

            migrationBuilder.DropIndex(
                name: "IX_Students_StudyGroupId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Lessons_StudyGroupId",
                table: "Lessons");

            migrationBuilder.DropIndex(
                name: "IX_Lessons_SubjectId",
                table: "Lessons");

            migrationBuilder.DropIndex(
                name: "IX_Lessons_TeacherId",
                table: "Lessons");

            migrationBuilder.DropColumn(
                name: "StudyGroupId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "StudyGroupId",
                table: "Lessons");
        }
    }
}
