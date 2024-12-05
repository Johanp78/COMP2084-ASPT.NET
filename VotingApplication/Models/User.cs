using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace VotingApplication.Models
{
    public class User : IdentityUser // Inheriting from IdentityUser
    {
 

        // Custom properties
        [Required]
        public int UserRol { get; set; } = 2; // Default value of 2
        public int UserElection { get; set; } = 1; // Default value of 1
        public string? UserImage { get; set; }
        public int UserStatus { get; set; } = 1; // Default value of 1

        // Navigation properties
        public Role? Role { get; set; }
        public Election? Election { get; set; }
        public ICollection<Candidate>? Candidates { get; set; } // Collection of Candidates for this User
    }
}
