using System;
using System.Collections.Generic;

namespace RecipeWebsite.Models;

public partial class Ingredient
{
    public int IdIngredient { get; set; }

    public string NameIngredient { get; set; } = null!;

    public virtual ICollection<RecipeIngredient> RecipeIngredients { get; set; } = new List<RecipeIngredient>();
}
