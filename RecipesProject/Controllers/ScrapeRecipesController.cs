using Microsoft.AspNetCore.Mvc;
using RecipesProject.Services;

namespace RecipesProject.Controllers
{
    public class ScrapeRecipesController : Controller
    {
        private readonly IGotvachBgScraperService gotvachBgScraperService;

        public ScrapeRecipesController(IGotvachBgScraperService gotvachBgScraperService)
        {
            this.gotvachBgScraperService = gotvachBgScraperService;
        }

        public IActionResult ScrapeRecipe()
        {
            return this.View();
        }

        public async Task<IActionResult> Add()
        {
            await this.gotvachBgScraperService.GetAllRecipesAsync(1193);

            return this.Redirect("/");
        }
    }
}
