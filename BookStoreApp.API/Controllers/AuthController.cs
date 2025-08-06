using AutoMapper;
using BookStoreApp.API.Data;
using BookStoreApp.API.Data.Models.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase //cip...30
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<ApiUser> _userManager;

        public AuthController(ILogger<AuthController> logger, IMapper mapper, UserManager<ApiUser> userManager) //cip...30
        {
            this._logger = logger;
            this._mapper = mapper;
            this._userManager = userManager;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(UserDto userDto)
        {
            /* not needed because .net will validate the model automatically via [Required] attributes
            if (userDto == null)
            {
                _logger.LogWarning("User registration attempted with null data.");
                return BadRequest("Invalid user data.");
            }*/
            // Registration logic here
            _logger.LogInformation($"Request to {nameof(Register)} with email: {userDto.Email}"); //cip...30
            try
            {
                var user = _mapper.Map<ApiUser>(userDto); // Assuming ApiUser is your user entity
                user.UserName = userDto.Email; // Set UserName to Email
                
                var result = await _userManager.CreateAsync(user, userDto.Password);

                if (result.Succeeded == false)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(error.Code, error.Description);
                    }
                    return BadRequest(ModelState);
                }
                //assign the role to the user
                await _userManager.AddToRoleAsync(user, userDto.Role);

                //return Ok(new { Message = "User registered successfully." });
                //return Accepted(new { Message = "User registered successfully." });
                return Accepted();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred in {nameof(Register)}.");
                return Problem($"An error occurred in {nameof(Register)}.", statusCode: 500);
            }
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginUserDto userDto)
        {
            _logger.LogInformation($"Request to {nameof(Login)} with email: {userDto.Email}"); //cip...30
            try
            {
                var user = await _userManager.FindByEmailAsync(userDto.Email);
                if (user == null)
                {
                    return Unauthorized("Invalid email or password.");
                }

                var result = await _userManager.CheckPasswordAsync(user, userDto.Password);
                if (!result)
                {
                    return Unauthorized("Invalid email or password.");
                }

                // Generate JWT token or any other authentication token here
                // For simplicity, returning a success message
                //return Ok(new { Message = "Login successful." });
                return Accepted(); // Return 202 Accepted for successful login
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred in {nameof(Login)}.");
                return Problem($"An error occurred in {nameof(Login)}.", statusCode: 500);
            }

        }
    }
}
