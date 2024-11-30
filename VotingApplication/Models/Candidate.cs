using static System.Collections.Specialized.BitVector32;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace VotingApplication.Models
{
    public class Candidate
    {
        [Key]
        public int CandidateId { get; set; } // Primary key

        [Required]
        [MaxLength(100)] // Optional: Limit the length of the name
        public string CandidateName { get; set; } // Name of the candidate

        public string UserId { get; set; } // Foreign key to the User who is the candidate

        public int ElectionId { get; set; } // Foreign key to the related Election

        // Navigation properties
        public User? User { get; set; } // Reference to the User entity
        public Election? Election { get; set; } // Reference to the Election entity
        public ICollection<Vote>? Votes { get; set; } // List of related votes
    }
}
