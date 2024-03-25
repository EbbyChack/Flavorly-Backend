using System;
using System.Collections.Generic;

namespace RecipeWebsite.Models;

public partial class UserFavorite
{
    public int IdUserFav { get; set; }

    public int IdUserFk { get; set; }

    public int IdRecipeFk { get; set; }

    public virtual Recipe IdRecipeFkNavigation { get; set; } = null!;

    public virtual User IdUserFkNavigation { get; set; } = null!;
}
