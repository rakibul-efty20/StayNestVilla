using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StayNestVilla_API.DTO;
using StayNestVilla_API.Services;

namespace StayNestVilla_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        private readonly IAuthService _authService = authService;

        [HttpGet]
        public async Task<ActionResult<ApiResponse<UserDTO>>> Register(RegistrationRequestDTO registrationRequestDTO)
        {
            try
            {
                if (registrationRequestDTO == null)
                {
                    return BadRequest(ApiResponse<object>.BadRequest("Registration data is required"));
                }

                if (await _authService.IsEmailExistsAsync(registrationRequestDTO.Email))
                {
                    return Conflict(
                        ApiResponse<object>.Conflict(
                            $"User with emial '{registrationRequestDTO.Email}' already exist"));
                }

                var user = await _authService.RegisterAsync(registrationRequestDTO);
                if (user == null)
                {
                    return BadRequest(ApiResponse<object>.BadRequest("Registration failed"));
                }

                //auth service 
                var response = ApiResponse<UserDTO>.CreatedAt(user, "User register  successfully");
                return CreatedAtAction(nameof(Register), response);
            }
            catch (Exception ex)
            {
                var errorResponse = ApiResponse<object>.Error(500, "An error occurred during registration", ex.Message);
                return StatusCode(500, errorResponse);
            }
            
        }
    }
}
