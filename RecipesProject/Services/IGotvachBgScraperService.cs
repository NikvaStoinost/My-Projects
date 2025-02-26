namespace RecipesProject.Services
{
    public interface IGotvachBgScraperService
    {
        Task GetAllRecipesAsync(int recipesCount);
    }
}
