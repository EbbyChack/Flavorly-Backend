using System;
using System.Collections.Generic;

namespace RecipeWebsite.Models;

public partial class Rating
{
    public int IdRating { get; set; }

    public int IdUserFk { get; set; }

    public int IdRecipeFk { get; set; }

    public int RatingValue { get; set; }

    public virtual Recipe IdRecipeFkNavigation { get; set; } = null!;

    public virtual User IdUserFkNavigation { get; set; } = null!;
}
