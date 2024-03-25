using System;
using System.Collections.Generic;

namespace RecipeWebsite.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public DateOnly DateOfBirth { get; set; }

    public string UserType { get; set; } = null!;

    public bool IsActive { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();

    public virtual ICollection<UserFavorite> UserFavorites { get; set; } = new List<UserFavorite>();
}
