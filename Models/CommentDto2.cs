namespace RecipeWebsite.Models
{
    public partial class CommentDto2
    {
       
        public int UserIdFk { get; set; }

  
        public int RecipeIdFk { get; set; }

        public string CommentText { get; set; } = null!;

        public DateTime DatePosted { get; set; }

        public bool IsActive { get; set; }
    }
}
