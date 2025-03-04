using Microsoft.EntityFrameworkCore;
using RecipesProject.Data;
using RecipesProject.Data.Models;
using RecipesProject.ViewModels.Recipes;

namespace RecipesProject.Services
{
    public class RecipeService : IRecipeService
    {
        private readonly ApplicationDbContext db;

        private string[] allowedExtentions = new[] { "jpg", "png", "gif" };

        public RecipeService(ApplicationDbContext db)
        {
            this.db = db;
            ;
        }

        public async Task CreateAsync(CreateRecipeInputModel input, string userId, string photoPath)
        {
            var recipe = new Recipe()
            {
                CategoryId = input.CategoryId,
                CookingTime = TimeSpan.FromMinutes(input.CookingTime),
                PreparationTime = TimeSpan.FromMinutes(input.PreparationTime),
                Title = input.Title,
                PortionsCount = input.PortionsCount,
                Description = input.Description,
                UserId = userId,
            };

            foreach (var inputingredient in input.Ingredients)
            {
                var ingredient = this.db.Ingredients.FirstOrDefault(x => x.Name == inputingredient.Name);

                if (ingredient == null)
                {
                    ingredient = new Ingredient { Name = inputingredient.Name };
                };

                recipe.RecipeIngredients.Add(new RecipeIngredient
                {
                    Ingredient = ingredient,
                    Quantity = inputingredient.Quantity
                });
            }

            Directory.CreateDirectory(Path.Combine(photoPath, "photos", "recipes"));

            foreach (var photo in input.Photo)
            {
                var extention = Path.GetExtension(photo.FileName).TrimStart('.');

                if (!allowedExtentions.Any(x => extention.EndsWith(x)))
                {
                    throw new Exception("Invalid photo exception");
                }

                var dbPhoto = new Photos
                {
                    UserId = userId,
                    Extension = extention,
                };
                recipe.Photos.Add(dbPhoto);

                var physicalPath = Path.Combine(photoPath, "photos", "recipes", $"{dbPhoto.Id}.{extention}");

                using (Stream fileStream = new FileStream(physicalPath, FileMode.Create))
                {
                    await photo.CopyToAsync(fileStream);
                }

                await this.db.Recipes.AddAsync(recipe);
                await this.db.SaveChangesAsync();
            }
        }

        public IEnumerable<RecipeInListViewModel> GetAllRecipes(int page, int itemsPerPage = 12)
        {
            var model = this.db.Recipes.OrderByDescending(x => x.Id).Skip((page - 1) * itemsPerPage)
                 .Take(itemsPerPage)
                 .Select(x => new RecipeInListViewModel
                 {
                     Id = x.Id,
                     Title = x.Title,
                     CategoryName = x.Category.Name,
                     CategoryId = x.Category.Id,
                     ImageUrl = x.Photos.FirstOrDefault() != null
                     ? x.Photos.FirstOrDefault().Extension
                     : "photos/recipes" + x.Photos.FirstOrDefault().Id + "." + x.Photos.FirstOrDefault().Extension,
                 }).ToList();

            return model;
        }

        public RecipeViewModel GetById(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid recipe ID");
            }

            var recipeId = this.db.Recipes
           .Include(r => r.Category) 
           .Include(r => r.User) 
           .Include(r => r.RecipeIngredients) 
           .ThenInclude(ri => ri.Ingredient) 
           .FirstOrDefault(x => x.Id == id);

            if (recipeId == null)
            {
                return null;
            }

            var recipe = new RecipeViewModel
            {
                Name = recipeId.Title,
                CategoryName = recipeId.Category != null ? recipeId.Category.Name : "Няма категория",
                AddedByUser = recipeId.User != null ? recipeId.User.UserName : "Неизвестен потребител",
                ImageUrl = recipeId.OriginalUrl,
                Description = recipeId.Description,
                PreparationTime = recipeId.PreparationTime,
                CookingTime = recipeId.CookingTime,
                PortionsCount = recipeId.PortionsCount,
                Ingredients = recipeId.RecipeIngredients != null
                 ? recipeId.RecipeIngredients.Select(x => new RecipeIngredientsViewModel
                 {
                     Name = x.Ingredient.Name,
                     Quantity = x.Quantity
                 }).ToList()
                 : new List<RecipeIngredientsViewModel>()
            };

            return recipe;
        }

        public int GetCount()
        {
            return this.db.Recipes.Count();
        }

        public IEnumerable<IndexPageRecipeViewModel> GetRandom(int count)
        {
            var recipes = this.db.Recipes
                .OrderBy(x => Guid.NewGuid())
                .Take(count)
                .Select(x => new IndexPageRecipeViewModel
                {
                    Id = x.Id,
                    RecipeModel = x.RecipeIngredients.Select(ri => new IndexRecipeViewModel
                    {
                        CategoryName = ri.Recipe.Category.Name,
                        ImageUrl = ri.Recipe.OriginalUrl,
                        Title = ri.Recipe.Title
                    }).ToList()
                })
                .ToList();

            return recipes;
        }
    }
}
