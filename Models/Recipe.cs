using System;
using System.Collections.Generic;

namespace RecipeWebsite.Models;

public partial class Recipe
{
    public int IdRecipe { get; set; }

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

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();

    public virtual ICollection<RecipeCategory> RecipeCategories { get; set; } = new List<RecipeCategory>();

    public virtual ICollection<RecipeIngredient> RecipeIngredients { get; set; } = new List<RecipeIngredient>();

    public virtual ICollection<UserFavorite> UserFavorites { get; set; } = new List<UserFavorite>();
}
