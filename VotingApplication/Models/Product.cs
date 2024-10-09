using System.ComponentModel.DataAnnotations;

namespace VotingApplication.Models
{
    public class Product
    {
        // Primary Key
        public int ProductId { get; set; }

        // Required attribute for Name
        [Required(ErrorMessage = "Product Name is required")]
        [StringLength(100, ErrorMessage = "Product Name cannot be longer than 100 characters")]
        public string Name { get; set; }

        // Optional Description field
        [StringLength(500, ErrorMessage = "Description cannot be longer than 500 characters")]
        public string? Description { get; set; }

        // Foreign Key to Category
        public int CategoryId { get; set; }

        // Navigation property for Category (Many-to-One relationship)
        public Category Category { get; set; }
    }
}
