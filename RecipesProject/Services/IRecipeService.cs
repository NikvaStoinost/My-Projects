using RecipesProject.ViewModels.Recipes;

namespace RecipesProject.Services
{
    public interface IRecipeService
    {
        Task CreateAsync(CreateRecipeInputModel input , string userId , string photoPath);

        IEnumerable<RecipeInListViewModel> GetAllRecipes(int page, int itemsPerPage = 12);

        int GetCount();

        IEnumerable<IndexPageRecipeViewModel> GetRandom(int count);

        RecipeViewModel GetById(int id);
    }
}
