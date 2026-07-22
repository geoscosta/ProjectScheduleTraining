using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectScheduleTraining.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSchedulingMakeupFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasMedicalCertificate",
                table: "Schedulings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "MakeupDeadline",
                table: "Schedulings",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasMedicalCertificate",
                table: "Schedulings");

            migrationBuilder.DropColumn(
                name: "MakeupDeadline",
                table: "Schedulings");
        }
    }
}
