using static System.Collections.Specialized.BitVector32;
using System.ComponentModel.DataAnnotations;

namespace VotingApplication.Models
{
    public class Candidate
    {
        [Key]
        public int CandidateId { get; set; }
        public string UserId { get; set; }
        public int ElectionElection { get; set; }

        public User User { get; set; }
        public Election Election { get; set; }
        public ICollection<Vote> Votes { get; set; }
    }
}
