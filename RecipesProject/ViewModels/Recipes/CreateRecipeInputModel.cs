using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RecipesProject.ViewModels.Recipes
{
    public class CreateRecipeInputModel
    {
        [Required]
        [MinLength(4)]
        public string Title { get; set; }

        [Required]
        [MinLength(20)]
        public string Description { get; set; }

        [Range(0, 24 * 60)]
        [Display(Name = "Preparation time (in minutes)")]
        public int PreparationTime { get; set; }

        [Range(0, 24 * 60)]
        [Display(Name = "Cooking time (in minutes)")]
        public int CookingTime { get; set; }

        [Range(1, 100)]
        public int PortionsCount { get; set; }

        [Display(Name = "Choose Category")]
        public int CategoryId { get; set; }

        public IEnumerable<IFormFile> Photo { get; set; }
    
        public IEnumerable<RecipeIngredientInputModel> Ingredients { get; set; }

        public IEnumerable<SelectListItem> CategoriesItems { get; set; }
    }
}
