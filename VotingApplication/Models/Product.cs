using System.ComponentModel.DataAnnotations;

namespace VotingApplication.Models
{

    public class Vote
    {
        public int VoteId { get; set; }  // Primary Key
        public string VoterName { get; set; }
        public string Candidate { get; set; }

        // Foreign Key to Election
        public int ElectionId { get; set; }
        public Election Election { get; set; }  // Navigation property
    }

}
