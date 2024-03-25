using System;
using System.Collections.Generic;

namespace RecipeWebsite.Models;

public partial class RecipeIngredient
{
    public int IdRecipeIng { get; set; }

    public int IdRecipeFk { get; set; }

    public int IdIngredientFk { get; set; }

    public virtual Ingredient IdIngredientFkNavigation { get; set; } = null!;

    public virtual Recipe IdRecipeFkNavigation { get; set; } = null!;
}
