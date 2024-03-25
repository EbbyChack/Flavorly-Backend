using System;
using System.Collections.Generic;

namespace RecipeWebsite.Models;

public partial class RecipeCategory
{
    public int IdRecipeCat { get; set; }

    public int IdRecipeFk { get; set; }

    public int IdCategoryFk { get; set; }

    public virtual Category IdCategoryFkNavigation { get; set; } = null!;

    public virtual Recipe IdRecipeFkNavigation { get; set; } = null!;
}
