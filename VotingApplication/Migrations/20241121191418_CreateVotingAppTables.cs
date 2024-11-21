using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VotingApplication.Migrations
{
    /// <inheritdoc />
    public partial class CreateVotingAppTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Elections",
                columns: table => new
                {
                    ElectionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ElectionTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ElectionStatus = table.Column<int>(type: "int", nullable: false),
                    ElectionStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ElectionStartHour = table.Column<TimeSpan>(type: "time", nullable: false),
                    ElectionEndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ElectionEndHour = table.Column<TimeSpan>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Elections", x => x.ElectionId);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    RolesId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RolesName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.RolesId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserRol = table.Column<int>(type: "int", nullable: false),
                    UserElection = table.Column<int>(type: "int", nullable: false),
                    UserImage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_Users_Elections_UserElection",
                        column: x => x.UserElection,
                        principalTable: "Elections",
                        principalColumn: "ElectionId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Users_Roles_UserRol",
                        column: x => x.UserRol,
                        principalTable: "Roles",
                        principalColumn: "RolesId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Candidates",
                columns: table => new
                {
                    CandidateId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ElectionElection = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Candidates", x => x.CandidateId);
                    table.ForeignKey(
                        name: "FK_Candidates_Elections_ElectionElection",
                        column: x => x.ElectionElection,
                        principalTable: "Elections",
                        principalColumn: "ElectionId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Candidates_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Votes",
                columns: table => new
                {
                    VotesId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VotesElection = table.Column<int>(type: "int", nullable: false),
                    VotesCandidate = table.Column<int>(type: "int", nullable: false),
                    VotesDatetime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Votes", x => x.VotesId);
                    table.ForeignKey(
                        name: "FK_Votes_Candidates_VotesCandidate",
                        column: x => x.VotesCandidate,
                        principalTable: "Candidates",
                        principalColumn: "CandidateId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Votes_Elections_VotesElection",
                        column: x => x.VotesElection,
                        principalTable: "Elections",
                        principalColumn: "ElectionId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Candidates_ElectionElection",
                table: "Candidates",
                column: "ElectionElection");

            migrationBuilder.CreateIndex(
                name: "IX_Candidates_UserId",
                table: "Candidates",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserElection",
                table: "Users",
                column: "UserElection");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserRol",
                table: "Users",
                column: "UserRol");

            migrationBuilder.CreateIndex(
                name: "IX_Votes_VotesCandidate",
                table: "Votes",
                column: "VotesCandidate");

            migrationBuilder.CreateIndex(
                name: "IX_Votes_VotesElection",
                table: "Votes",
                column: "VotesElection");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Votes");

            migrationBuilder.DropTable(
                name: "Candidates");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Elections");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
