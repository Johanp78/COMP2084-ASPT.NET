using System.ComponentModel.DataAnnotations;

namespace VotingApplication.Models
{
    //public class Category
    //{
    //    //Primary key

    //    public int CategoryId {get; set;}

    //    //Category Name
    //    [Required(ErrorMessage = "Category Name is required")]
    //    public string Name {get; set;}

    //    public 

    //} 

    public class Election
    {
        public int ElectionId { get; set; }  // Primary Key
        public string ElectionName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        // Navigation property
        public ICollection<Vote> Votes { get; set; }
    }

}


