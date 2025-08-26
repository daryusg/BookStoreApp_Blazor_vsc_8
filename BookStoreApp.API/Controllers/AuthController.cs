using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using BookStoreApp.API.Data;
using BookStoreApp.API.Data.Models.User;
using BookStoreApp.API.Static;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace BookStoreApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(typeof(AuthResponse), 200)] //OK
    [ProducesResponseType(typeof(AuthResponse), 201)] //Created 
    [ProducesResponseType(typeof(AuthResponse), 202)] //Accepted
    [ProducesResponseType(204)] //No Content
    public class AuthController : ControllerBase //cip...30
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<ApiUser> _userManager;
        private readonly IConfiguration _configuration; //cip...32

        public AuthController(ILogger<AuthController> logger, IMapper mapper, UserManager<ApiUser> userManager, IConfiguration configuration) //cip...30,32
        {
            this._logger = logger;
            this._mapper = mapper;
            this._userManager = userManager;
            this._configuration = configuration;
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
                return Accepted(); // Return 202 Accepted for successful login
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred in {nameof(Register)}.");
                return Problem($"An error occurred in {nameof(Register)}.", statusCode: 500);
            }
        }

        [HttpPost]
        [Route("login")]
        //public async Task<IActionResult> Login(LoginUserDto userDto)
        public async Task<ActionResult<AuthResponse>> Login(LoginUserDto userDto) //cip...32
        {
            _logger.LogInformation($"Request to {nameof(Login)} with email: {userDto.Email}"); //cip...30
            try
            {
                var user = await _userManager.FindByEmailAsync(userDto.Email);
                if (user == null)
                {
                    return Unauthorized(userDto);
                }

                var result = await _userManager.CheckPasswordAsync(user, userDto.Password);
                if (!result)
                {
                    return Unauthorized(userDto);
                }

                // Generate JWT token or any other authentication token here //genned by copilot
                // For simplicity, returning a success message //genned by copilot
                //var tokenString = await _userManager.GenerateUserTokenAsync(user, TokenOptions.DefaultProvider, "Login"); //genned by copilot
                string tokenString = await GenerateTokenAsync(user);

                var response = new AuthResponse
                {
                    UserId = user.Id,
                    Token = tokenString,
                    Email = userDto.Email
                };

                //return Ok(new { Message = "Login successful." }); //genned by copilot
                return Accepted(response); // Return 202 Accepted for successful login. NOTE: fails in blazor client genned by nswagger...fix [ProducesResponseType(202)]
                //return Ok(response); // Return 200 OK for successful login
                //could also: return response; // due to Task<ActionResult<AuthResponse>>
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred in {nameof(Login)}.");
                return Problem($"An error occurred in {nameof(Login)}.", statusCode: 500);
            }

        }

        private async Task<string> GenerateTokenAsync(ApiUser user) //cip...32
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = roles.Select(role => new Claim(ClaimTypes.Role, role)).ToList();
            //adding db claims
            var userClaims = await _userManager.GetClaimsAsync(user);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
                // Add any other claims you need
                new Claim(CustomClaimTypes.Uid, user.Id)
            }
            .Union(userClaims) // Add user claims from the database
            .Union(roleClaims);

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(_configuration.GetValue<int>("JwtSettings:Duration")),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
