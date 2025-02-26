using System.ComponentModel.DataAnnotations;

namespace RecipesProject.ViewModels.Recipes
{
    public class RecipeIngredientInputModel
    {
        [Required]
        [MinLength(3)]
        public string Name { get; set; }

        [Required]
        [MinLength(1)]
        public string Quantity { get; set; }
    }
}
