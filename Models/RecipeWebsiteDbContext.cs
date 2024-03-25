using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace RecipeWebsite.Models;

public partial class RecipeWebsiteDbContext : DbContext
{
    public RecipeWebsiteDbContext()
    {
    }

    public RecipeWebsiteDbContext(DbContextOptions<RecipeWebsiteDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<Ingredient> Ingredients { get; set; }

    public virtual DbSet<Rating> Ratings { get; set; }

    public virtual DbSet<Recipe> Recipes { get; set; }

    public virtual DbSet<RecipeCategory> RecipeCategories { get; set; }

    public virtual DbSet<RecipeIngredient> RecipeIngredients { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserFavorite> UserFavorites { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=.\\SQLEXPRESS;Initial Catalog=RecipeWebsiteDB;Integrated Security=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.IdCategory);
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.IdComment);

            entity.Property(e => e.DatePosted).HasColumnType("datetime");
            entity.Property(e => e.RecipeIdFk).HasColumnName("RecipeIdFK");
            entity.Property(e => e.UserIdFk).HasColumnName("UserIdFK");

            entity.HasOne(d => d.RecipeIdFkNavigation).WithMany(p => p.Comments)
                .HasForeignKey(d => d.RecipeIdFk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Comments_Recipes");

            entity.HasOne(d => d.UserIdFkNavigation).WithMany(p => p.Comments)
                .HasForeignKey(d => d.UserIdFk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Comments_Users");
        });

        modelBuilder.Entity<Ingredient>(entity =>
        {
            entity.HasKey(e => e.IdIngredient);
        });

        modelBuilder.Entity<Rating>(entity =>
        {
            entity.HasKey(e => e.IdRating);

            entity.Property(e => e.IdRecipeFk).HasColumnName("IdRecipeFK");
            entity.Property(e => e.IdUserFk).HasColumnName("IdUserFK");

            entity.HasOne(d => d.IdRecipeFkNavigation).WithMany(p => p.Ratings)
                .HasForeignKey(d => d.IdRecipeFk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ratings_Recipes");

            entity.HasOne(d => d.IdUserFkNavigation).WithMany(p => p.Ratings)
                .HasForeignKey(d => d.IdUserFk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ratings_Users");
        });

        modelBuilder.Entity<Recipe>(entity =>
        {
            entity.HasKey(e => e.IdRecipe);

            entity.Property(e => e.VideoUrl).HasColumnName("VideoURL");
        });

        modelBuilder.Entity<RecipeCategory>(entity =>
        {
            entity.HasKey(e => e.IdRecipeCat);

            entity.Property(e => e.IdCategoryFk).HasColumnName("IdCategoryFK");
            entity.Property(e => e.IdRecipeFk).HasColumnName("IdRecipeFK");

            entity.HasOne(d => d.IdCategoryFkNavigation).WithMany(p => p.RecipeCategories)
                .HasForeignKey(d => d.IdCategoryFk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RecipeCategories_Categories");

            entity.HasOne(d => d.IdRecipeFkNavigation).WithMany(p => p.RecipeCategories)
                .HasForeignKey(d => d.IdRecipeFk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RecipeCategories_Recipes");
        });

        modelBuilder.Entity<RecipeIngredient>(entity =>
        {
            entity.HasKey(e => e.IdRecipeIng);

            entity.Property(e => e.IdIngredientFk).HasColumnName("IdIngredientFK");
            entity.Property(e => e.IdRecipeFk).HasColumnName("IdRecipeFK");

            entity.HasOne(d => d.IdIngredientFkNavigation).WithMany(p => p.RecipeIngredients)
                .HasForeignKey(d => d.IdIngredientFk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RecipeIngredients_Ingredients");

            entity.HasOne(d => d.IdRecipeFkNavigation).WithMany(p => p.RecipeIngredients)
                .HasForeignKey(d => d.IdRecipeFk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RecipeIngredients_Recipes");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.UserType).HasMaxLength(50);
        });

        modelBuilder.Entity<UserFavorite>(entity =>
        {
            entity.HasKey(e => e.IdUserFav);

            entity.Property(e => e.IdRecipeFk).HasColumnName("IdRecipeFK");
            entity.Property(e => e.IdUserFk).HasColumnName("IdUserFK");

            entity.HasOne(d => d.IdRecipeFkNavigation).WithMany(p => p.UserFavorites)
                .HasForeignKey(d => d.IdRecipeFk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserFavorites_Recipes");

            entity.HasOne(d => d.IdUserFkNavigation).WithMany(p => p.UserFavorites)
                .HasForeignKey(d => d.IdUserFk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserFavorites_Users");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
