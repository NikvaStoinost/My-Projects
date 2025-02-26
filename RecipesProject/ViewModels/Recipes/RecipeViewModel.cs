namespace RecipesProject.ViewModels.Recipes
{
    public class RecipeViewModel
    {
        public string Name { get; set; }

        public string CategoryName { get; set; }

        public string AddedByUser { get; set; }

        public string ImageUrl { get; set; }

        public string Description { get; set; }

        public TimeSpan PreparationTime { get; set; }

        public TimeSpan CookingTime { get; set; }

        public int PortionsCount { get; set; }

        public IEnumerable<RecipeIngredientsViewModel> Ingredients { get; set; }
    }
}
