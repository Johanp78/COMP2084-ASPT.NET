using System.ComponentModel.DataAnnotations;

namespace VotingApplication.Models
{
    public class Role
    {
        [Key]
        public int RolesId { get; set; }
        [Required]
        public string RolesName { get; set; }

        // Navigation properties
        public ICollection<User> Users { get; set; }
    }
}
