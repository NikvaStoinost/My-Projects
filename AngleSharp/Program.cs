using AngleSharp;
using AngleSharp.Dom;
using System;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        // Конфигурация
        var config = Configuration.Default.WithDefaultLoader();
        var address = "https://gotvach.bg/";

        // Зареждане на документа
        var context = BrowsingContext.New(config);
        var document = await context.OpenAsync(address);

        // Извличане на данни с CSS селектори
        var recipes = document.QuerySelectorAll("a.title");
        foreach (var recipe in recipes)
        {
            var title = recipe.TextContent.Trim();
            var link = recipe.GetAttribute("href");
            Console.WriteLine($"Title: {title}");
            Console.WriteLine($"Link: https://gotvach.bg{link}");
            Console.WriteLine("-----------------------");
        }
    }
}