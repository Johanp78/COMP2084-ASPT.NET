using System.ComponentModel.DataAnnotations;

namespace VotingApplication.Models
{
    public class Election
    {
        [Key]
        public int ElectionId { get; set; }
        [Required]
        public string ElectionTitle { get; set; }
        [Required]
        public int ElectionStatus { get; set; }
        [Required]
        public DateTime ElectionStartDate { get; set; }
        [Required]
        public DateTime ElectionEndDate { get; set; }

        // Navigation properties
        public ICollection<User>? Users { get; set; }
        public ICollection<Candidate>? Candidates { get; set; }
        public ICollection<Vote>? Votes { get; set; }
    }
}
