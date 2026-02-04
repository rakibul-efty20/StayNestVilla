using System.ComponentModel.DataAnnotations;

namespace StayNestVilla_API.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        [MaxLength(100)]
        public required string Username { get; set; }

        [Required] 
        public required string Passward { get; set; }

        [Required]
        [MaxLength(50)]
        public required string Role { get; set; } = "Customer";

        public DateTime CreatedDate { get; set; } 

        public DateTime UpdatedDate { get; set; } 

    }
}
