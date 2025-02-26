using Microsoft.CodeAnalysis.CSharp.Syntax;
using PuppeteerSharp;
using RecipesProject.Data;
using RecipesProject.Data.Models;
using RecipesProject.Dto_s;
using System.Collections.Concurrent;
using System.Globalization;
using System.Text;

namespace RecipesProject.Services
{
    public class GotvachBgScraperService : IGotvachBgScraperService
    {
        private readonly ApplicationDbContext db;

        public GotvachBgScraperService(ApplicationDbContext db)
        {
            this.db = db;  
        }

        public async Task GetAllRecipesAsync(int recipesCount)
        {
            var concurentBag = new ConcurrentBag<RecipeDto>();

            Parallel.For(1178, recipesCount, (i) =>
            {
                try
                {
                    var recipe = this.GetRecipe(i);
                    concurentBag.Add(recipe);
                }
                catch
                {

                }
                            
            });

            foreach (var recipe in concurentBag)
            {
                var categoryId = await this.GetOrCreateCategoryAsync(recipe.CategoryName);
                var recipeExists = this.db.Recipes.Any(x => x.Title == recipe.RecipeName);

                if (recipeExists)
                {
                    continue;
                }

                var newRecipe = new Recipe
                {
                    Title = recipe.RecipeName,
                    Description = recipe.Description,
                    PreparationTime = recipe.PreparationTime,
                    CookingTime = recipe.CookingTime,
                    PortionsCount = recipe.PortionsCount,
                    OriginalUrl = recipe.OriginalUrl,
                    CategoryId = categoryId,
                };

               // if (newRecipe.OriginalUrl == null)
               // {
               //     continue;
               // }

                await this.db.Recipes.AddAsync(newRecipe);
                await this.db.SaveChangesAsync();

                foreach (var item in recipe.Ingredients)
                {
                    var ingredient = item.Split("-");

                    if (ingredient.Length < 2)
                    {
                        continue;
                    }

                    var ingredientsId = await GetOrCreateIngredientAsync(ingredient[0].Trim());
                    var quantity = ingredient[1].Trim();

                    var recipeIngredient = new RecipeIngredient
                    {
                        IngredientId = ingredientsId,
                        RecipeId = newRecipe.Id,
                        Quantity = quantity,
                    };

                    await this.db.RecipeIngredients.AddAsync(recipeIngredient);
                }

                var photo = new Photos
                {
                    Extension = recipe.OriginalUrl,
                    RecipeId = newRecipe.Id
                };

                await this.db.Photos.AddAsync(photo);
                await this.db.SaveChangesAsync();
            }
            await this.db.SaveChangesAsync();
        }

        private async Task<int> GetOrCreateIngredientAsync(string ingredientName)
        {
            var ingredient = this.db.Ingredients.FirstOrDefault(x => x.Name == ingredientName);

            foreach (var name in ingredientName)
            {               

                if (ingredient == null)
                {
                    ingredient = new Ingredient
                    {
                        Name = ingredientName,
                    };

                    await this.db.Ingredients.AddAsync(ingredient);
                    await this.db.SaveChangesAsync();
                }

            }

            return ingredient.Id;
        }

        private async Task<int> GetOrCreateCategoryAsync(string categoryName)
        {
            var category = this.db.Categories.FirstOrDefault(x => x.Name == categoryName);

            if (category == null)
            {
                category = new Category
                {
                    Name = categoryName,
                };
              await this.db.Categories.AddAsync(category);
              await this.db.SaveChangesAsync();
            }

            return category.Id;
        }

        public  RecipeDto GetRecipe(int id)
        {
            new BrowserFetcher().DownloadAsync().GetAwaiter().GetResult();
            


           IBrowser browser = Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = true
            }).GetAwaiter().GetResult();

            IPage page = browser.NewPageAsync().GetAwaiter().GetResult();

           page.SetUserAgentAsync
                ("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36").GetAwaiter().GetResult();

            var url = $"https://recepti.gotvach.bg/r-{id}";

            var response = page.GoToAsync(url).GetAwaiter().GetResult();

            if (response.Status == System.Net.HttpStatusCode.NotFound)
            {
                throw new InvalidOperationException();
            }

            var recipe = new RecipeDto();

           var getCategoryName = page.EvaluateExpressionAsync<string>(
                "document.querySelector('#recEntity  > div.brdc').innerText"
                ).GetAwaiter().GetResult();

           var splitCategoryNames = getCategoryName.Split(new[] { '>', '»' }, StringSplitOptions.RemoveEmptyEntries)
                                                  .Select(name => name.Trim())
                                                  .Reverse()
                                                  .ToList();

            recipe.CategoryName = splitCategoryNames[1];
            recipe.RecipeName = splitCategoryNames[0];

            var getDescriptionText = page.EvaluateExpressionAsync<string[]>(
                 @"Array.from(document.querySelectorAll('div.text p')).map(p => p.innerText)"
             ).GetAwaiter().GetResult();

            StringBuilder sb = new StringBuilder();

            foreach (var item in getDescriptionText)
            {
               sb.AppendLine(item);
            }

            recipe.Description = sb.ToString().TrimEnd();

            var cookingAndPreparationTime = page.EvaluateExpressionAsync<string[]>(
                     @"
                const indiDivs = Array.from(document.querySelectorAll('.indi > div'));
                const times = indiDivs.slice(0, 2).map(div => div.innerText.trim());
                times;
            ").GetAwaiter().GetResult();

            var preparationTime = cookingAndPreparationTime[0]
                .Replace("Приготвяне", string.Empty).Trim()
                .Replace("мин.", string.Empty).Trim();

            recipe.PreparationTime = TimeSpan
                .ParseExact(preparationTime, "mm", CultureInfo.InvariantCulture);

            var cookingTime = cookingAndPreparationTime[1]
                .Replace("Готвене", string.Empty).Trim()
                .Replace("мин.", string.Empty).Trim();

            recipe.CookingTime = TimeSpan
              .ParseExact(cookingTime, "mm", CultureInfo.InvariantCulture);
            ;
            var portionsAndMinutes = page.EvaluateExpressionAsync<string>(
                       @"
                const icbFakDiv = document.querySelector('#recContent > div.acin > div.indi > div.icb-fak');
                const textContent = icbFakDiv ? icbFakDiv.innerText.trim() : '';
                textContent;
            ").GetAwaiter().GetResult();

            var portionsCount = portionsAndMinutes
                .Replace("Порции", string.Empty);

            recipe.PortionsCount = int.Parse(portionsCount);

               recipe.OriginalUrl = page.EvaluateExpressionAsync<string>(
                       @"
                const imageElement = document.querySelector('#gall img'); 
                const imageSrc = imageElement ? imageElement.src : ''; 
                imageSrc;
            ").GetAwaiter().GetResult();


            var ingredients = page.EvaluateExpressionAsync<string[]>(
               @"Array.from(document.querySelectorAll('section.products.new[data-role=""rls_app""] ul li'))
              .map(li => li.innerText.trim())").GetAwaiter().GetResult();

            foreach (var item in ingredients)
            {
                recipe.Ingredients.Add(item);
            }

            return recipe;
        }
    }
}
