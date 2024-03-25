using System;
using System.Collections.Generic;

namespace RecipeWebsite.Models;

public partial class Category
{
    public int IdCategory { get; set; }

    public string CategoryName { get; set; } = null!;

    public virtual ICollection<RecipeCategory> RecipeCategories { get; set; } = new List<RecipeCategory>();
}
