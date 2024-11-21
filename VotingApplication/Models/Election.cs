using System.ComponentModel.DataAnnotations;

namespace VotingApplication.Models
{
    public class Election
    {
        [Key]
        public int ElectionId { get; set; }
        [Required]
        public string ElectionTitle { get; set; }
        public int ElectionStatus { get; set; }
        public DateTime ElectionStartDate { get; set; }
        public TimeSpan ElectionStartHour { get; set; }
        public DateTime ElectionEndDate { get; set; }
        public TimeSpan ElectionEndHour { get; set; }

        // Navigation properties
        public ICollection<User> Users { get; set; }
        public ICollection<Candidate> Candidates { get; set; }
        public ICollection<Vote> Votes { get; set; }
    }
}
