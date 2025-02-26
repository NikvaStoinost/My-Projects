namespace RecipesProject.ViewModels.Recipes
{
    public class IndexPageRecipeViewModel
    {
        public int Id { get; set; }
        
        public IEnumerable<IndexRecipeViewModel> RecipeModel { get; set; }
    }
}
