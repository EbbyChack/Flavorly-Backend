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
        [HttpGet]
        public async Task<ActionResult<User>> GetUserSettings()
        {
            // Get the user id from the User property
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }

            // Find the user in the database
            var user = await _context.Users.FindAsync(int.Parse(userId));
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);

        }

       
    }
}
