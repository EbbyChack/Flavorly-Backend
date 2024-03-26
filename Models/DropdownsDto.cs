namespace RecipeWebsite.Models
{
    public class DropdownsDto
    {
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<Ingredient> Ingredients { get; set; }
    }
}
