using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace RecipeWebsite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        //pippo
        private readonly RecipeWebsiteDbContext _context;

        public AuthController(RecipeWebsiteDbContext context)
        {
            _context = context;
        }


        // POST /api/auth/register
        [HttpPost("register")]
        public ActionResult<User> Register(UserDto userDto)
        {
            // Check if the username already exists
            if (_context.Users.Any(u => u.Username == userDto.Username))
                return BadRequest("Username is already taken");

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password);

            User user = new User
            {
                Username = userDto.Username,
                Password = passwordHash,
                Email = userDto.Email,
                Name = userDto.Name,
                Surname = userDto.Surname,
                DateOfBirth = userDto.DateOfBirth,
                UserType = "User",
                IsActive = true

            };

            // Save the user to the database
            _context.Users.Add(user);
            _context.SaveChanges();

            return Ok(user);
        }

        // POST /api/auth/login
        [HttpPost("login")]
        public ActionResult<User> Login(LoginDto loginDto)
        {
            // Find the user by username
            var user = _context.Users.SingleOrDefault(u => u.Username == loginDto.Username);

            // Check if the user exists and if the password is correct
            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password))
            {
                return BadRequest("Invalid username or password");
            }

            string token = CreateToken(user);
            return Ok(token);

        }

      

        private string CreateToken(User user)
        {
            // Create the claims
            List<Claim> claims = new List<Claim>
            {

                // Add the user id to the claims
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                // Add the username to the claims
                new Claim(ClaimTypes.Name, user.Username),
                // Add the role to the claims
                new Claim(ClaimTypes.Role, user.UserType)
            };

            // Create the key using the secret key
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("recipewebsitejwttokenkey12345678"));

            // Create the signing credentials using the key and the HMACSHA256 algorithm
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Create the JWT token
            var token = new JwtSecurityToken(   
                claims: claims,
                expires: DateTime.Now.AddDays(7),
                signingCredentials: creds
                );
            
            // Write the token to a string
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;

        }
    }
}
