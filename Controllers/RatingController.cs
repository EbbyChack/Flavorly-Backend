using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace RecipeWebsite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingController : ControllerBase
    {
        private readonly RecipeWebsiteDbContext _context;

        public RatingController(RecipeWebsiteDbContext context)
        {
            _context = context;
        }

        //POST /api/rating
        //A USER CAN ONLY RATE A RECIPE ONCE!!!!!
        [HttpPost]
        public async Task<ActionResult<Rating>> CreateRating(RatingDto ratingDto)
        {
            //dont forget to multiply the rating value by 2 in the front end
            if (ratingDto.RatingValue < 1 || ratingDto.RatingValue > 10)
            {
                return BadRequest("Rating value must be between 1 and 10");
            }


            Rating rating = new Rating
            {
                IdUserFk = ratingDto.IdUserFk,
                IdRecipeFk = ratingDto.IdRecipeFk,
                RatingValue = ratingDto.RatingValue
                
            };

            _context.Ratings.Add(rating);
            await _context.SaveChangesAsync();

            return Ok(rating);
        }

        // PUT /api/rating/5
        // need to insert the rating id in the url
        [HttpPut("{id}")]
        public async Task<ActionResult<Rating>> UpdateRating(int id, RatingDto ratingDto)
        {
            if (ratingDto.RatingValue < 1 || ratingDto.RatingValue > 10)
            {
                return BadRequest("Rating value must be between 1 and 10");
            }

            var rating = await _context.Ratings.FindAsync(id);

            if (rating == null)
            {
                return NotFound();
            }

            rating.RatingValue = ratingDto.RatingValue;

            await _context.SaveChangesAsync();

            return Ok(rating);
        }

        //AVEARGE RATING
        //GET /api/rating/average/5
        //need to insert the recipe id in the url
        [HttpGet("average/{id}")]
        public async Task<ActionResult<int>> GetAverageRating(int id)
        {
            var ratings = await _context.Ratings
                                .Where(r => r.IdRecipeFk == id)
                                .ToListAsync();

            if (ratings.Count == 0)
            {
                return NotFound("No ratings found for this recipe.");
            }
            //round the average rating to the nearest whole number
            var averageRating = (int)Math.Round(ratings.Average(r => r.RatingValue));
            var numberOfRatings = ratings.Count;

            var result = new RatingResultDto
            {
                AverageRating = averageRating,
                NumberOfRatings = numberOfRatings
            };

            return Ok(result);
        }


    }
}
