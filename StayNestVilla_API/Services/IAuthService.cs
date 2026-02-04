using StayNestVilla_API.DTO;
using LoginRequest = Microsoft.AspNetCore.Identity.Data.LoginRequest;

namespace StayNestVilla_API.Services
{
    public interface IAuthService
    {
        Task<UserDTO?> RegisterAsync(RegistrationRequestDTO registrationRequestDTO);

        Task<LoginResponseDTO> LoginAsync(LoginRequestDTO loginRequestDTO);

        Task<bool> IsEmailExistsAsync(string email);

    }
}
