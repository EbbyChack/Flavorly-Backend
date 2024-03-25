using System;
using System.Collections.Generic;

namespace RecipeWebsite.Models;

public partial class Comment
{
    public int IdComment { get; set; }

    public int UserIdFk { get; set; }

    public int RecipeIdFk { get; set; }

    public string CommentText { get; set; } = null!;

    public DateTime DatePosted { get; set; }

    public bool IsActive { get; set; }

    public virtual Recipe RecipeIdFkNavigation { get; set; } = null!;

    public virtual User UserIdFkNavigation { get; set; } = null!;
}
