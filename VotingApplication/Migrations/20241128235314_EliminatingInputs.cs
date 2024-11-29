using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VotingApplication.Migrations
{
    /// <inheritdoc />
    public partial class EliminatingInputs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ElectionEndHour",
                table: "Elections");

            migrationBuilder.DropColumn(
                name: "ElectionStartHour",
                table: "Elections");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<TimeSpan>(
                name: "ElectionEndHour",
                table: "Elections",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "ElectionStartHour",
                table: "Elections",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));
        }
    }
}
