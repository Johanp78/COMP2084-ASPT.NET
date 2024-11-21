using System.ComponentModel.DataAnnotations;

namespace VotingApplication.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string UserEmail { get; set; }
        [Required]
        public string Password { get; set; }
        public int UserRol { get; set; }
        public int UserElection { get; set; }
        public string UserImage { get; set; }
        public int UserStatus { get; set; }

        // Navigation properties
        public Role Role { get; set; }
        public Election Election { get; set; }
        public ICollection<Candidate> Candidates { get; set; }
    }
}
