
using System.ComponentModel.DataAnnotations;

namespace RecipesProject.Data.Models
{
    public class Recipe
    {
        public Recipe()
        {
            this.Id = Id;
            this.RecipeIngredients = new HashSet<RecipeIngredient>();
            this.Comments = new HashSet<Comment>();
            this.Photos = new HashSet<Photos>();
        }

        [Key]
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public TimeSpan PreparationTime { get; set; }

        public TimeSpan CookingTime { get; set; }

        public int PortionsCount { get; set; }

        public string? OriginalUrl { get; set; }

        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }

        public string? UserId { get; set; }

        public virtual CustomApplicationUser User { get; set; } 

        public ICollection<RecipeIngredient> RecipeIngredients { get; set; }

        public ICollection<Comment> Comments { get; set; }

        public ICollection<Photos> Photos { get; set; }
    }
}
