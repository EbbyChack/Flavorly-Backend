namespace RecipeWebsite.Models
{
    public partial class CommentDto
    {
        public int IdComment { get; set; }
        public int UserIdFk { get; set; }

        public string Username { get; set; }

        public int RecipeIdFk { get; set; }

        public string CommentText { get; set; } = null!;

        public DateTime DatePosted { get; set; }

        public bool IsActive { get; set; }
    }
}
