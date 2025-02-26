using PuppeteerSharp;
using System.Text;

namespace ConsoleApp1
{
    public class Evala() 
    {

        public static async Task Main()
        {
            await new BrowserFetcher().DownloadAsync();

            IBrowser browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true });

            IPage page = await browser.NewPageAsync();

            browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = true
            });


            var url = $"https://recepti.gotvach.bg/r-120";

            var response = await page.GoToAsync(url);

            if (response.Status == System.Net.HttpStatusCode.NotFound)
            {
                throw new InvalidOperationException();
            }

            // var recipe = new RecipeDto();

            var getCategoryName = await page.EvaluateExpressionAsync<string>(
                 "document.querySelector('#recEntity  > div.brdc').innerText"
                 );

            var splitCategoryNames = getCategoryName.Split(new[] { '>', '»' }, StringSplitOptions.RemoveEmptyEntries)
                                                   .Select(name => name.Trim())
                                           .Reverse()
                                           .ToList();
         //  Console.WriteLine(splitCategoryNames[1]);
         //  Console.WriteLine(splitCategoryNames[2]);
            //  recipe.CategoryName = splitCategoryNames[1];
            //  recipe.RecipeName = splitCategoryNames[0];

            var getDescriptionText = await page.EvaluateExpressionAsync<string[]>(
                 @"Array.from(document.querySelectorAll('div.text p')).map(p => p.innerText)"
             );

            StringBuilder sb = new StringBuilder();

            foreach (var item in getDescriptionText)
            {
                sb.AppendLine(item);
            }

            //  recipe.Description = sb.ToString().TrimEnd();

            var cookingAndPreparationTime = await page.EvaluateExpressionAsync<string[]>(
                     @"
        const indiDivs = Array.from(document.querySelectorAll('.indi > div'));
        const times = indiDivs.slice(0, 2).map(div => div.innerText.trim());
        times;
    ");

            var preparationTime = cookingAndPreparationTime[0]
                .Replace("Приготвяне", string.Empty).Trim()
                .Replace("мин.", string.Empty).Trim();

            //   recipe.PreparationTime = TimeSpan
            //  .ParseExact(preparationTime, "mm", CultureInfo.InvariantCulture);

            var cookingTime = cookingAndPreparationTime[1]
                .Replace("Готвене", string.Empty).Trim()
                .Replace("мин.", string.Empty).Trim();

            //  recipe.CookingTime = TimeSpan
            //    .ParseExact(cookingTime, "mm", CultureInfo.InvariantCulture);
            ;
            var portionsAndMinutes = await page.EvaluateExpressionAsync<string>(
                       @"
                    const icbFakDiv = document.querySelector('#recContent > div.acin > div.indi > div.icb-fak');
                    const textContent = icbFakDiv ? icbFakDiv.innerText.trim() : '';
                    textContent;
                ");

            var portionsCount = portionsAndMinutes
                .Replace("Порции", string.Empty);

            // recipe.PortionsCount = int.Parse(portionsCount);

            var imageUrl = await page.EvaluateExpressionAsync<string>(
                       @"
                    const imageElement = document.querySelector('#gall img'); 
                    const imageSrc = imageElement ? imageElement.src : ''; 
                    imageSrc;
                "
            );


            var ingredients = await page.EvaluateExpressionAsync<string[]>(
               @"Array.from(document.querySelectorAll('section.products.new[data-role=""rls_app""] ul li'))
                  .map(li => li.innerText.trim())");

            foreach (var item in ingredients)
            {
                var ingredientInfo = item.Split(" - ");


                var name = ingredientInfo[0];

                if (ingredientInfo.Length < 2)
                {
                    continue;
                }

                   var quantity = ingredientInfo[1];
                   Console.WriteLine(quantity);



                // recipe.IngredientName.Add(name);
                // recipe.IngredientQuantity.Add(quantity);
            }
        }

    }
}