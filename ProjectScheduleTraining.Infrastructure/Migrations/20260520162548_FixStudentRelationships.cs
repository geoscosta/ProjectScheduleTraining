using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectScheduleTraining.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixStudentRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Enrollments_Students_StudentId1",
                table: "Enrollments");

            migrationBuilder.DropForeignKey(
                name: "FK_Financials_Students_StudentId1",
                table: "Financials");

            migrationBuilder.DropForeignKey(
                name: "FK_Schedulings_Students_StudentId1",
                table: "Schedulings");

            migrationBuilder.DropIndex(
                name: "IX_Schedulings_StudentId1",
                table: "Schedulings");

            migrationBuilder.DropIndex(
                name: "IX_Financials_StudentId1",
                table: "Financials");

            migrationBuilder.DropIndex(
                name: "IX_Enrollments_StudentId1",
                table: "Enrollments");

            migrationBuilder.DropColumn(
                name: "StudentId1",
                table: "Schedulings");

            migrationBuilder.DropColumn(
                name: "StudentId1",
                table: "Financials");

            migrationBuilder.DropColumn(
                name: "StudentId1",
                table: "Enrollments");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "StudentId1",
                table: "Schedulings",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "StudentId1",
                table: "Financials",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "StudentId1",
                table: "Enrollments",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Schedulings_StudentId1",
                table: "Schedulings",
                column: "StudentId1");

            migrationBuilder.CreateIndex(
                name: "IX_Financials_StudentId1",
                table: "Financials",
                column: "StudentId1");

            migrationBuilder.CreateIndex(
                name: "IX_Enrollments_StudentId1",
                table: "Enrollments",
                column: "StudentId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Enrollments_Students_StudentId1",
                table: "Enrollments",
                column: "StudentId1",
                principalTable: "Students",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Financials_Students_StudentId1",
                table: "Financials",
                column: "StudentId1",
                principalTable: "Students",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Schedulings_Students_StudentId1",
                table: "Schedulings",
                column: "StudentId1",
                principalTable: "Students",
                principalColumn: "Id");
        }
    }
}
