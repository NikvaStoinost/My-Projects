using System.ComponentModel.DataAnnotations;

namespace RecipesProject.Data.Models
{
    public class RecipeIngredient
    {
        [Key]
        public int RecipeId { get; set; }

        public virtual Recipe Recipe { get; set; }

        [Key]
        public int IngredientId { get; set; }

        public virtual Ingredient Ingredient { get; set; }  

        public string Quantity { get; set; }
    }
}
