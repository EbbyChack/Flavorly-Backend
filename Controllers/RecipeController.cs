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
        public ActionResult<IEnumerable<Recipe>> GetRecipes()
        {
            var recipes = _context.Recipes
                                  .Include(r => r.RecipeCategories)
                                  .Include(r => r.RecipeIngredients)
                                  .ToList();
            return Ok(recipes);
        }

        // GET /api/recipe/5
        [HttpGet("{id}")]
        public ActionResult<Recipe> GetRecipe(int id)
        {
            var recipe = _context.Recipes
                                  .Where(r => r.IdRecipe == id)
                                  .Include(r => r.RecipeCategories)
                                  .Include(r => r.RecipeIngredients);

            if (recipe == null)
                return NotFound();

            return Ok(recipe);
        }

        // POST /api/recipe
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
                IsActive = recipeDto.IsActive,

            };

            recipe.RecipeCategories = recipeDto.RecipeCategoriesIds.Select(id => new RecipeCategory
            {
                IdCategoryFk = id,
                IdRecipeFkNavigation = recipe  // Set the navigation property
            }).ToList();

            recipe.RecipeIngredients = recipeDto.RecipeIngredientsIds.Select(id => new RecipeIngredient
            {
                IdIngredientFk = id,
                IdRecipeFkNavigation = recipe  // Set the navigation property
            }).ToList();

            _context.Recipes.Add(recipe);
            await _context.SaveChangesAsync();

            return Ok(recipe);


        }

        // PUT /api/recipe/5
        [HttpPut("edit/{id}")]
        public async Task<ActionResult<Recipe>> UpdateRecipe(int id, Recipe r)
        {
            var recipe = await _context.Recipes
                                      .Include(rc => rc.RecipeCategories)
                                      .Include(ri => ri.RecipeIngredients)
                                      .FirstOrDefaultAsync(r => r.IdRecipe == id);

            if (recipe == null)
                return NotFound();

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

            // Add new RecipeCategories
            var recipeCategoriesToAdd = r.RecipeCategories.Where(rc => !recipe.RecipeCategories.Select(rc => rc.IdCategoryFk).Contains(rc.IdCategoryFk)).ToList();
            foreach (var rc in recipeCategoriesToAdd)
            {
                recipe.RecipeCategories.Add(rc);
            }

            // Add new RecipeIngredients
            var recipeIngredientsToAdd = r.RecipeIngredients.Where(ri => !recipe.RecipeIngredients.Select(ri => ri.IdIngredientFk).Contains(ri.IdIngredientFk)).ToList();
            foreach (var ri in recipeIngredientsToAdd)
            {
                recipe.RecipeIngredients.Add(ri);
            }

            await _context.SaveChangesAsync();

            return Ok();
        }

    }
}
