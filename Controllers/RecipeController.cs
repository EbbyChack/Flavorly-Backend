using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipeWebsite.Models;

namespace RecipeWebsite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipeController : ControllerBase
    {
        private readonly RecipeWebsiteDbContext _context;

        public RecipeController(RecipeWebsiteDbContext context)
        {
            _context = context;
        }

        // GET /api/recipe
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Recipe>>> GetRecipes()
        {
            var recipes = await _context.Recipes
                                  .Include(r => r.RecipeCategories)
                                  .Include(r => r.RecipeIngredients)
                                  .Where(r => r.IsActive)
                                  .ToListAsync();
            return Ok(recipes);
        }

        // GET /api/recipe/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Recipe>> GetRecipe(int id)
        {
            var recipe = await _context.Recipes
                               .Include(r => r.RecipeCategories)
                               .Include(r => r.RecipeIngredients)                        
                               .Include(r => r.Ratings)
                               .Include(r => r.UserFavorites)
                               .Where(r => r.IsActive)
                               .Select(r => new Recipe
                               {
                                   IdRecipe = r.IdRecipe,
                                   NameRecipe = r.NameRecipe,
                                   Description = r.Description,
                                   CookingTime = r.CookingTime,
                                   Servings = r.Servings,
                                   Difficulty = r.Difficulty,
                                   Instructions = r.Instructions,
                                   MainImg = r.MainImg,
                                   Img2 = r.Img2,
                                   Img3 = r.Img3,
                                   VideoUrl = r.VideoUrl,
                                   DateAdded = r.DateAdded,
                                   IsActive = r.IsActive,
                                   RecipeCategories = r.RecipeCategories,
                                   RecipeIngredients = r.RecipeIngredients,

                                   //include only the active comments
                                   Comments = r.Comments.Where(c => c.IsActive).ToList(),
                                   Ratings = r.Ratings,
                                   UserFavorites = r.UserFavorites,
 
                               })
                               .FirstOrDefaultAsync(r => r.IdRecipe == id);

            if (recipe == null)
                return NotFound();

            return Ok(recipe);
        }

        // POST /api/recipe
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<Recipe>> CreateRecipe(RecipeDto recipeDto)
        {
            Recipe recipe = new Recipe
            {
                NameRecipe = recipeDto.NameRecipe,
                Description = recipeDto.Description,
                CookingTime = recipeDto.CookingTime,
                Servings = recipeDto.Servings,
                Difficulty = recipeDto.Difficulty,
                Instructions = recipeDto.Instructions,
                MainImg = recipeDto.MainImg,
                Img2 = recipeDto.Img2,
                Img3 = recipeDto.Img3,
                VideoUrl = recipeDto.VideoUrl,
                DateAdded = recipeDto.DateAdded,
                IsActive = true,
                

            };

            recipe.RecipeCategories = recipeDto.RecipeCategoriesIds.Select(id => new RecipeCategory
            {
                IdCategoryFk = id,    
            }).ToList();

            recipe.RecipeIngredients = recipeDto.RecipeIngredientsIds.Select(id => new RecipeIngredient
            {
                IdIngredientFk = id,              
            }).ToList();

            _context.Recipes.Add(recipe);
            await _context.SaveChangesAsync();

            return Ok(recipe);


        }

        // PUT /api/recipe/5
        [Authorize(Roles = "Admin")]
        [HttpPut("edit/{id}")]
        public async Task<ActionResult<Recipe>> UpdateRecipe(int id, RecipeEditDto r)
        {
            var recipe = await _context.Recipes
                                      .Include(rc => rc.RecipeCategories)
                                      .Include(ri => ri.RecipeIngredients)
                                      .FirstOrDefaultAsync(r => r.IdRecipe == id);

            if (recipe == null)
                return NotFound();

            // Update the scalar properties
            recipe.NameRecipe = r.NameRecipe;
            recipe.Description = r.Description;
            recipe.CookingTime = r.CookingTime;
            recipe.Servings = r.Servings;
            recipe.Difficulty = r.Difficulty;
            recipe.Instructions = r.Instructions;
            recipe.MainImg = r.MainImg;
            recipe.Img2 = r.Img2;
            recipe.Img3 = r.Img3;
            recipe.VideoUrl = r.VideoUrl;
            recipe.DateAdded = r.DateAdded;
            recipe.IsActive = r.IsActive;

            // Update the RecipeCategories
            _context.RecipeCategories.RemoveRange(recipe.RecipeCategories);
            recipe.RecipeCategories = r.RecipeCategoriesIds.Select(id => new RecipeCategory
            {
                IdCategoryFk = id,
                IdRecipeFkNavigation = recipe
            }).ToList();

            // Update the RecipeIngredients
            _context.RecipeIngredients.RemoveRange(recipe.RecipeIngredients);
            recipe.RecipeIngredients = r.RecipeIngredientsIds.Select(id => new RecipeIngredient
            {
                IdIngredientFk = id,
                IdRecipeFkNavigation = recipe
            }).ToList();

            await _context.SaveChangesAsync();

            return Ok();
        }


        //soft delete
        [Authorize(Roles = "Admin")]
        [HttpPut("delete/{id}")]
        public async Task<ActionResult<Recipe>> DeleteRecipe(int id)
        {
            var recipe = await _context.Recipes.FirstOrDefaultAsync(r => r.IdRecipe == id);

            if (recipe == null)
                return NotFound();

            recipe.IsActive = false;

            await _context.SaveChangesAsync();

            return Ok();
        }

        // GET /api/recipe/dropdowns
        [Authorize(Roles = "Admin")]
        [HttpGet("dropdowns")]
        public async Task<ActionResult<DropdownsDto>> GetDropdowns()
        {
            var categories = await _context.Categories.ToListAsync();
            var ingredients = await _context.Ingredients.ToListAsync();

            var dropdowns = new DropdownsDto
            {
                Categories = categories,
                Ingredients = ingredients
            };

            return Ok(dropdowns);
        }

        // GET THE RECIPES WITH THE HIGHEST RATING
        [HttpGet("top")]
        public async Task<ActionResult<IEnumerable<Recipe>>> GetTopRecipes()
        {
            var recipes = await _context.Recipes
                                  .Include(r => r.Ratings)
                                  .Where(r => r.IsActive)
                                  .OrderByDescending(r => r.Ratings.Average(r => r.RatingValue))      
                                  .Take(10)
                                  .ToListAsync();

            return Ok(recipes);
        }

    }
}
