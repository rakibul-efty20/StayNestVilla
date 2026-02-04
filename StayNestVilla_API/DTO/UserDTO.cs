using System.ComponentModel.DataAnnotations;

namespace StayNestVilla_API.DTO
{
    public class UserDTO
    {
   
        public int Id { get; set; }

        public required string Email { get; set; } = default!;

        public required string Username { get; set; }

        public required string Role { get; set; } 
    }
}
