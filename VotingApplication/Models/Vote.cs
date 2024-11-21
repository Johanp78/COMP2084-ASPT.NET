using static System.Collections.Specialized.BitVector32;
using System.ComponentModel.DataAnnotations;

namespace VotingApplication.Models
{
    public class Vote
    {
        [Key]
        public int VotesId { get; set; }
        public int VotesElection { get; set; }
        public int VotesCandidate { get; set; }
        public DateTime VotesDatetime { get; set; } = DateTime.Now;

        // Navigation properties
        public Election Election { get; set; }
        public Candidate Candidate { get; set; }
    }
}
