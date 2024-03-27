namespace RecipeWebsite.Models
{
    public partial class UpdatePasswordDto
    {
        public string OldPassword { get; set; } = null!;

        public string NewPassword { get; set; } = null!;
    }
}
