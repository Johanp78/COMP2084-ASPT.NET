using System.ComponentModel.DataAnnotations;

namespace VotingApplication.Models
{
    public class Candidate
    {
        [Key]
        public int CandidateId { get; set; }

        [Required]
        [Display(Name = "Candidate Name")]
        public string? CandidateName { get; set; } 

        public string? UserId { get; set; }
        public int ElectionId { get; set; }

        // Navigation properties
        public User? User { get; set; }
        public Election? Election { get; set; }
        public ICollection<Vote>? Votes { get; set; }
    }
}
