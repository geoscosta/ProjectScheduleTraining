using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectScheduleTraining.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddStudentExtendedFeatures : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ParQAssessments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AssessmentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Question1 = table.Column<bool>(type: "bit", nullable: false),
                    Question2 = table.Column<bool>(type: "bit", nullable: false),
                    Question3 = table.Column<bool>(type: "bit", nullable: false),
                    Question4 = table.Column<bool>(type: "bit", nullable: false),
                    Question5 = table.Column<bool>(type: "bit", nullable: false),
                    Question6 = table.Column<bool>(type: "bit", nullable: false),
                    Question7 = table.Column<bool>(type: "bit", nullable: false),
                    IsCleared = table.Column<bool>(type: "bit", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParQAssessments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ParQAssessments_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ScheduleLocks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EnrollmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LockStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LockEndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Justification = table.Column<int>(type: "int", nullable: false),
                    DocumentUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    Semester = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduleLocks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScheduleLocks_Enrollments_EnrollmentId",
                        column: x => x.EnrollmentId,
                        principalTable: "Enrollments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ScheduleLocks_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StudentContracts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EnrollmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SignedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SignatureHash = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    SignatureIp = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: true),
                    ContractContent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PdfUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsSigned = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentContracts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentContracts_Enrollments_EnrollmentId",
                        column: x => x.EnrollmentId,
                        principalTable: "Enrollments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StudentContracts_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StudentMeasures",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MeasureDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Weight = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    Height = table.Column<decimal>(type: "decimal(4,2)", nullable: false),
                    Bmi = table.Column<decimal>(type: "decimal(4,2)", nullable: false),
                    ChestCircumference = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    WaistCircumference = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    HipCircumference = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    ArmCircumference = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    ThighCircumference = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    CalfCircumference = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    BodyFatPercentage = table.Column<decimal>(type: "decimal(4,2)", nullable: true),
                    LeanMassPercentage = table.Column<decimal>(type: "decimal(4,2)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentMeasures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentMeasures_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudentWorkouts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TrainerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentWorkouts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentWorkouts_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentWorkouts_Users_TrainerId",
                        column: x => x.TrainerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WorkoutExercises",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WorkoutId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MuscleGroup = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Sets = table.Column<int>(type: "int", nullable: false),
                    Repetitions = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Load = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    RestSeconds = table.Column<int>(type: "int", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Order = table.Column<int>(type: "int", nullable: false),
                    VideoUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkoutExercises", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkoutExercises_StudentWorkouts_WorkoutId",
                        column: x => x.WorkoutId,
                        principalTable: "StudentWorkouts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ParQAssessments_StudentId",
                table: "ParQAssessments",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleLocks_EnrollmentId",
                table: "ScheduleLocks",
                column: "EnrollmentId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleLocks_StudentId",
                table: "ScheduleLocks",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentContracts_EnrollmentId",
                table: "StudentContracts",
                column: "EnrollmentId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentContracts_StudentId",
                table: "StudentContracts",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentMeasures_StudentId",
                table: "StudentMeasures",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentWorkouts_StudentId",
                table: "StudentWorkouts",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentWorkouts_TrainerId",
                table: "StudentWorkouts",
                column: "TrainerId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutExercises_WorkoutId",
                table: "WorkoutExercises",
                column: "WorkoutId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ParQAssessments");

            migrationBuilder.DropTable(
                name: "ScheduleLocks");

            migrationBuilder.DropTable(
                name: "StudentContracts");

            migrationBuilder.DropTable(
                name: "StudentMeasures");

            migrationBuilder.DropTable(
                name: "WorkoutExercises");

            migrationBuilder.DropTable(
                name: "StudentWorkouts");
        }
    }
}
