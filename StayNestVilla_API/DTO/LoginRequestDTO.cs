using System.ComponentModel.DataAnnotations;

namespace StayNestVilla_API.DTO
{
    public class LoginRequestDTO
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        public required string Passward { get; set; }
    }
}
