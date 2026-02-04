using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StayNestVilla_API.DataBase;
using StayNestVilla_API.DTO;
using StayNestVilla_API.Models;

namespace StayNestVilla_API.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public AuthService(ApplicationDbContext db, IConfiguration configuration, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }
        public async Task<UserDTO?> RegisterAsync(RegistrationRequestDTO registrationRequestDTO)
        {
            try
            {
                if (await IsEmailExistsAsync(registrationRequestDTO.Email))
                {
                    throw new InvalidOperationException(
                        $"User with email '{registrationRequestDTO.Email}' alerady exist");
                }

                User user = new()
                {
                    Email = registrationRequestDTO.Email,
                    Username = registrationRequestDTO.Username,
                    Passward = registrationRequestDTO.Passward,
                    Role = string.IsNullOrEmpty(registrationRequestDTO.Role) ? "Customer" : registrationRequestDTO.Role,
                    CreatedDate = DateTime.UtcNow
                };
                await _db.Users.AddAsync(user);
                await _db.SaveChangesAsync();

                return _mapper.Map<UserDTO>(user);
            }
            catch(Exception ex)
            {
                throw new InvalidOperationException("An unexpected error occured during user registration", ex);
            }
          
        }

        public Task<LoginResponseDTO> LoginAsync(LoginRequestDTO loginRequestDTO)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> IsEmailExistsAsync(string email)
        {
            return await _db.Users.AnyAsync(u => u.Email.Equals(email, StringComparison.CurrentCultureIgnoreCase));
        }
    }
}
