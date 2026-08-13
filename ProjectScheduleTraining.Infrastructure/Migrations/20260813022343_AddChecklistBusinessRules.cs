using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectScheduleTraining.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddChecklistBusinessRules : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CancellationOption",
                table: "Enrollments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "CancellationPenaltyAmount",
                table: "Enrollments",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ContractorId",
                table: "Enrollments",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SubstituteStudentId",
                table: "Enrollments",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Contractor",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cnpj = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contractor", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Enrollments_ContractorId",
                table: "Enrollments",
                column: "ContractorId");

            migrationBuilder.CreateIndex(
                name: "IX_Enrollments_SubstituteStudentId",
                table: "Enrollments",
                column: "SubstituteStudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Enrollments_Contractor_ContractorId",
                table: "Enrollments",
                column: "ContractorId",
                principalTable: "Contractor",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Enrollments_Students_SubstituteStudentId",
                table: "Enrollments",
                column: "SubstituteStudentId",
                principalTable: "Students",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Enrollments_Contractor_ContractorId",
                table: "Enrollments");

            migrationBuilder.DropForeignKey(
                name: "FK_Enrollments_Students_SubstituteStudentId",
                table: "Enrollments");

            migrationBuilder.DropTable(
                name: "Contractor");

            migrationBuilder.DropIndex(
                name: "IX_Enrollments_ContractorId",
                table: "Enrollments");

            migrationBuilder.DropIndex(
                name: "IX_Enrollments_SubstituteStudentId",
                table: "Enrollments");

            migrationBuilder.DropColumn(
                name: "CancellationOption",
                table: "Enrollments");

            migrationBuilder.DropColumn(
                name: "CancellationPenaltyAmount",
                table: "Enrollments");

            migrationBuilder.DropColumn(
                name: "ContractorId",
                table: "Enrollments");

            migrationBuilder.DropColumn(
                name: "SubstituteStudentId",
                table: "Enrollments");
        }
    }
}
