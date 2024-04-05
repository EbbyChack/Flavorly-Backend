using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace RecipeWebsite.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserSettingsController : ControllerBase
    {
        private readonly RecipeWebsiteDbContext _context;

        public UserSettingsController(RecipeWebsiteDbContext context)
        {
            _context = context;
        }

        // GET /api/usersettings
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUserSettings(int id)
        {
            // Find the user in the database
            var user = await _context.Users.Where(u => u.UserId == id).Select(u => new 
            {
                UserId = u.UserId,
                Username = u.Username,
                Email = u.Email,
                Name = u.Name,
                Surname = u.Surname,
                DateOfBirth = u.DateOfBirth,

                // Include the user's comments
                Comments = u.Comments.Where(c => c.IsActive),
                Ratings = u.Ratings
                
            }).FirstOrDefaultAsync();

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);

        }

        //Update password
        // PUT /api/usersettings/updatepassword
        [HttpPut("updatepassword/{id}")]
        public async Task<ActionResult<User>> UpdatePassword(int id,UpdatePasswordDto updatePasswordDto)
        {
           

            // Find the user in the database
            var user = await _context.Users.Where(u => u.UserId == id).FirstOrDefaultAsync();

            if (user == null)
            {
                return NotFound();
            }

            // Check if the old password is correct
            if (!BCrypt.Net.BCrypt.Verify(updatePasswordDto.OldPassword, user.Password))
            {
                return BadRequest("Old password is incorrect");
            }

            // Update the password
            user.Password = BCrypt.Net.BCrypt.HashPassword(updatePasswordDto.NewPassword);

            await _context.SaveChangesAsync();

            return Ok(user);
        }
    }
}
