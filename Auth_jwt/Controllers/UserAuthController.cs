using Auth_jwt.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Auth_jwt.Models;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Auth_jwt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAuthController : ControllerBase
    {
        private UserManager<ApplicationUser> _usermanager;
        private SignInManager<ApplicationUser> _signinManager;
        private string? _jwtKey;
        private string? _JwtIssuer;
        private string? _JwtAudience;
        private int _JwtExpiry;

        public UserAuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signinManager, IConfiguration configuration)
        {
            _usermanager = userManager;
            _signinManager = signinManager;
            _jwtKey = configuration["Jwt:Key"];
            _JwtIssuer = configuration["Jwt:Issuer"];
            _JwtAudience = configuration["Jwt:Audience"];
            _JwtExpiry = int.Parse(configuration["Jwt:ExpiryMinutes"]);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel registerModel)
        {
            if(registerModel == null
                || string.IsNullOrEmpty(registerModel.Name)
                || string.IsNullOrEmpty(registerModel.Email)
                || string.IsNullOrEmpty(registerModel.Password))
            {
                return BadRequest("Invalid data");
            }

            var exsisitingUser = await _usermanager.FindByEmailAsync(registerModel.Email);

            if (exsisitingUser != null)
            {
                return BadRequest("User already exists");
            }

            var user = new ApplicationUser
            {
                Name = registerModel.Name,
                Email = registerModel.Email,
                UserName = registerModel.Email
            };

            var result = await _usermanager.CreateAsync(user, registerModel.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok("New user created!");
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            var user = await _usermanager.FindByEmailAsync(loginModel.Email);

            if (user == null)
            {
                return Unauthorized(new {succes = false, message = "Invalid username or password"});
            }

            var result = await _signinManager.CheckPasswordSignInAsync(user, loginModel.Password, false);

            if (!result.Succeeded)
            {
                return Unauthorized(new { succes = false, message = "Invalid username or password" });
            }

            var token = GenerateJwtToken(user);

            return Ok(new { succes = true, token });
        }

        private string GenerateJwtToken(ApplicationUser user)
        {
            var Claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("Name", user.Name)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtKey));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: Claims,
                expires: DateTime.Now.AddMinutes(_JwtExpiry),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            await _signinManager.SignOutAsync();
            return Ok("User logged out!");
        }
    }
}
