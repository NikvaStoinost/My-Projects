using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecipesProject.Services;
using RecipesProject.ViewModels.Recipes;
using System.Security.Claims;

namespace RecipesProject.Controllers
{
    public class RecipesController : Controller
    {
        private readonly ICategoriesService categoriesService;
        private readonly IRecipeService recipeService;
        private readonly IWebHostEnvironment environment;

        public RecipesController(ICategoriesService categoriesService, IRecipeService recipeService , IWebHostEnvironment environment)
        {
            this.categoriesService = categoriesService;
            this.recipeService = recipeService;
            this.environment = environment;
        }

        public async Task<IActionResult> Create()
        {
            var viewmodel = new CreateRecipeInputModel();
            viewmodel.CategoriesItems = this.categoriesService.GetKeyValuePairs();
            return this.View(viewmodel);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(CreateRecipeInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                input.CategoriesItems = this.categoriesService.GetKeyValuePairs();
                return this.View(input);
            }

            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            try
            {
               await this.recipeService.CreateAsync(input , userId , $"{this.environment.WebRootPath}/photos");
            }
            catch (Exception ex)
            {
                this.ModelState.AddModelError(string.Empty, ex.Message);
                input.CategoriesItems = this.categoriesService.GetKeyValuePairs();
                return this.View(input);
            }

            return this.Redirect("/");
        }

        public IActionResult All(int id = 1)
        {
            const int ItemsPerPage = 12;

            var viewModel = new RecipesListViewModel()
            {
                ItemsPerPage = ItemsPerPage,
                PageNumber = id,
                RecipesCount = this.recipeService.GetCount(),
                Recipes = this.recipeService.GetAllRecipes(id, 12),
            };

            return this.View(viewModel);
        }

        public IActionResult ById(int id)
        {
            var recipe = this.recipeService.GetById(id);
            return this.View(recipe);
        }

    }
}
