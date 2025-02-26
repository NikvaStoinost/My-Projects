using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using RecipesProject.Data.Models;

namespace RecipesProject.Data
{
    public class ApplicationDbContext : IdentityDbContext<CustomApplicationUser>
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Recipe> Recipes { get; set; }

        public DbSet<Ingredient> Ingredients { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<Photos> Photos { get; set; }

        public DbSet<RecipeIngredient> RecipeIngredients { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.ConfigureWarnings(warnings =>
                   warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
        }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);



            var user = new CustomApplicationUser
            {
                Id = "1",
                UserName = "seeduser",
                Email = "seeduser@example.com",
                EmailConfirmed = true
            };

            builder.Entity<CustomApplicationUser>().HasData(user);

            var category = new Category
            {
                Id = 1,
                Name = "Default Category"
            };

            builder.Entity<Category>().HasData(category);


            var recipe = new Recipe
            {
                Id = 1,
                Title = "Seed Recipe",
                Description = "This is a seed recipe",
                CookingTime = TimeSpan.Zero,
                PreparationTime = TimeSpan.Zero,
                PortionsCount = 4,
                CategoryId = 1,
                UserId = "1",
                OriginalUrl = "1",
            };

            builder.Entity<Recipe>().HasData(recipe);

            builder.Entity<RecipeIngredient>().HasKey(x => new { x.RecipeId, x.IngredientId });
        }

    }
}

    
