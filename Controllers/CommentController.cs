using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace RecipeWebsite.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly RecipeWebsiteDbContext _context;

        public CommentController(RecipeWebsiteDbContext context)
        {
            _context = context;
        }

        // GET /api/comment/5
        //to get all comments for a recipe
        //need to insert the recipe id in the url
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Comment>>> GetComments(int id)
        {
            var comments = await _context.Comments
                                  .Where(c => c.RecipeIdFk == id) 
                                  .Where(c => c.IsActive)
                                  .ToListAsync();
                                 
            return Ok(comments);
        }

        // POST /api/comment
        [HttpPost]
        public async Task<ActionResult<Comment>> CreateComment(CommentDto2 commentDto)
        {
            Comment comment = new Comment
            {
                UserIdFk = commentDto.UserIdFk,
                RecipeIdFk = commentDto.RecipeIdFk,
                CommentText = commentDto.CommentText,
                DatePosted = DateTime.Now,
                IsActive = true
                
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return Ok(comment);
        }

        //soft delete
        //Put /api/comment/5
        //need to insert the comment id in the url
        [HttpPut("delete/{id}")]
        public async Task<ActionResult<Comment>> DeleteComment(int id)
        {
            var comment = await _context.Comments.FindAsync(id);

            if (comment == null)
                return NotFound();

            comment.IsActive = false;
            await _context.SaveChangesAsync();

            return Ok(comment);
        }

    }
}
