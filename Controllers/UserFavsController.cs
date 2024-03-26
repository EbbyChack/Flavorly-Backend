using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace RecipeWebsite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserFavsController : ControllerBase
    {
        private readonly RecipeWebsiteDbContext _context;

        public UserFavsController(RecipeWebsiteDbContext context)
        {
            _context = context;
        }

       
        // Get all user favorites, searches by user id
        // GET /api/userfavs/5
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserFavorite>>> GetFavsOfUser(int id)
        {
            var favs = await _context.UserFavorites
                                  .Include(f => f.IdRecipeFkNavigation)
                                  .Where(f => f.IdUserFk == id)
                                  .Where(Ok => Ok.IdRecipeFkNavigation.IsActive)
                                  .ToListAsync();
            return Ok(favs);
            
        }

        // POST /api/userfavs
        [HttpPost]
        public async Task<ActionResult<UserFavorite>> CreateUserFav(UserFavDto userFavorite)
        {
            UserFavorite fav = new UserFavorite
            {
                IdUserFk = userFavorite.IdUserFk,
                IdRecipeFk = userFavorite.IdRecipeFk
            };

            _context.UserFavorites.Add(fav);
            await _context.SaveChangesAsync();

            return Ok();
            
        }

        // DELETE /api/userfavs/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<UserFavorite>> DeleteUserFav(int id)
        {
            var fav = await _context.UserFavorites.FindAsync(id);

            if (fav == null)
                return NotFound();

            _context.UserFavorites.Remove(fav);
            await _context.SaveChangesAsync();

            return Ok();
        }


        

        

       

        
    }
}
