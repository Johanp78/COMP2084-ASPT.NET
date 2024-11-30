using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VotingApplication.Migrations
{
    /// <inheritdoc />
    public partial class AddVoteEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
        IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Votes' AND xtype='U')
        BEGIN
            CREATE TABLE [Votes] (
                [VotesId] int NOT NULL IDENTITY,
                [VotesElection] int NOT NULL,
                [VotesCandidate] int NOT NULL,
                [VotesDatetime] datetime2 NOT NULL DEFAULT (GETDATE()),
                CONSTRAINT [PK_Votes] PRIMARY KEY ([VotesId]),
                CONSTRAINT [FK_Votes_Elections] FOREIGN KEY ([VotesElection]) REFERENCES [Elections] ([ElectionId]) ON DELETE CASCADE,
                CONSTRAINT [FK_Votes_Candidates] FOREIGN KEY ([VotesCandidate]) REFERENCES [Candidates] ([CandidateId]) ON DELETE CASCADE
            );
        END
    ");
        }



        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "Votes");
        }

    }
}
