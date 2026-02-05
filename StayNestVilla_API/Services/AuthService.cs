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

        public async Task<LoginResponseDTO> LoginAsync(LoginRequestDTO loginRequestDTO)
        {
            try
            {
                var user =
                    await _db.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == loginRequestDTO.Email.ToLower());

                if (user == null || user.Passward != loginRequestDTO.Passward)
                {
                    return null;
                }

                //generate jwt token


                return new LoginResponseDTO()
                {
                    UserDTO = _mapper.Map<UserDTO>(user),
                    Token = ""
                };
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An unexpected error occured during user login", ex);
            }

        }

        public async Task<bool> IsEmailExistsAsync(string email)
        {
            return await _db.Users.AnyAsync(u => u.Email.ToLower()==email.ToLower());
        }
    }
}
