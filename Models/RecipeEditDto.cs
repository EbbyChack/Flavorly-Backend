namespace RecipeWebsite.Models
{
    public partial class RecipeEditDto
    {
        

        public string NameRecipe { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string CookingTime { get; set; } = null!;

        public int Servings { get; set; }

        public string Difficulty { get; set; } = null!;

        public string Instructions { get; set; } = null!;

        public string MainImg { get; set; } = null!;

        public string Img2 { get; set; } = null!;

        public string Img3 { get; set; } = null!;

        public string? VideoUrl { get; set; }

        public DateOnly DateAdded { get; set; }

        public bool IsActive { get; set; }



        public ICollection<int> RecipeIngredientsIds { get; set; }

        public ICollection<int> RecipeCategoriesIds { get; set; }


    }
}

