namespace RecipeWebsite.Models
{
    public partial class RatingDto
    {
        public int IdUserFk { get; set; }

        public int IdRecipeFk { get; set; }

        public int RatingValue { get; set; }
    }
}
