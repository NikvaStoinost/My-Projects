using PuppeteerSharp;

class Program
{
    static async Task Main(string[] args)
    {
        for (int i = 1; i < 150000; i++)
        {
            await GetRecipe(i);
        }
    }

    static async Task  GetRecipe(int id)
    {
        await new BrowserFetcher().DownloadAsync();

        var browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true });

        var page = await browser.NewPageAsync();

        browser = await Puppeteer.LaunchAsync(new LaunchOptions
        {
            Headless = true
        });


        var url = $"https://recepti.gotvach.bg/r-{id}";

        var response = await page.GoToAsync(url);

        if (response.Status == System.Net.HttpStatusCode.NotFound)
        {
            Console.WriteLine($"{id} not found");
            return;
        }
        else
        {
            Console.WriteLine($"{id} found");
        }

        //Get Title 
        var getCategoryName = await page.EvaluateExpressionAsync<string>(
            "document.querySelector('#recEntity  > div.brdc').innerText"
            );

        var splitCategoryNames = getCategoryName.Split(new[] { '>', '»' }, StringSplitOptions.RemoveEmptyEntries)
                                              .Select(name => name.Trim())
                                              .Reverse()
                                              .ToList();

        var categoryName = splitCategoryNames[1];
      //  Console.WriteLine(categoryName);

        //Get RecipeName
        var ingredients = await page.EvaluateExpressionAsync<string[]>(
           @"Array.from(document.querySelectorAll('section.products.new[data-role=""rls_app""] ul li'))
              .map(li => li.innerText.trim())");

        var recipeName = splitCategoryNames[0];
      //  Console.WriteLine(recipeName);


        //Get Description
        var getDescriptionText = await page.EvaluateExpressionAsync<string[]>(
             @"Array.from(document.querySelectorAll('div.text p')).map(p => p.innerText)"
         );

        foreach (var item in getDescriptionText)
        {
            Console.WriteLine(item);
        }

        //Get PreparationTime And Cooking Time
        var cookingAndPreparationTime = await page.EvaluateExpressionAsync<string[]>(
                 @"
                const indiDivs = Array.from(document.querySelectorAll('.indi > div'));
                const times = indiDivs.slice(0, 2).map(div => div.innerText.trim());
                times;
            ");

        var preparationTime = cookingAndPreparationTime[0]
            .Replace("Приготвяне", string.Empty)
            .Replace("мин.", string.Empty);

       // Console.WriteLine(preparationTime);
        var cookingTime = cookingAndPreparationTime[1]
            .Replace("Готвене", string.Empty)
            .Replace("мин.", string.Empty);

      //  Console.WriteLine(cookingTime);

        //Get PortionsCount 
        var portionsAndMinutes = await page.EvaluateExpressionAsync<string>(
                   @"
                const icbFakDiv = document.querySelector('#recContent > div.acin > div.indi > div.icb-fak');
                const textContent = icbFakDiv ? icbFakDiv.innerText.trim() : '';
                textContent;
            ");

        var portionsCount = portionsAndMinutes
            .Replace("Порции", string.Empty);

       // Console.WriteLine(portionsCount);


        //Get Image Url
        var imageUrl = await page.EvaluateExpressionAsync<string>(
                   @"
                const imageElement = document.querySelector('#gall img'); 
                const imageSrc = imageElement ? imageElement.src : ''; 
                imageSrc;
            ");


       // Console.WriteLine(imageUrl);
    }
}