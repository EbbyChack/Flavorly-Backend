namespace RecipeWebsite.Models
{
    public class UserDto
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string UserType { get; set; } = "User";
        public bool IsActive { get; set; } = true;
    }
}
