using Microsoft.AspNetCore.Mvc;
using RecipesProject.Services;
using RecipesProject.ViewModels;
using RecipesProject.ViewModels.Recipes;
using System.Diagnostics;

namespace RecipesProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRecipeService recipeService;

        public HomeController(ILogger<HomeController> logger , IRecipeService recipeService )
        {
            _logger = logger;
            this.recipeService = recipeService;
        }

        public IActionResult Index()
        {
            var indexRecipe = this.recipeService.GetRandom(10);

            return View(indexRecipe);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}