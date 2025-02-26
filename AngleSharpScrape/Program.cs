using AngleSharp;

public class Program
{
    static async Task Main(string[] args)
    {
        var config = Configuration.Default.WithDefaultLoader();
        var context = BrowsingContext.New(config);

        var document = await context.OpenAsync("https://recepti.gotvach.bg/r-279350");

        var elements = document.QuerySelectorAll("li");

        foreach (var item in elements)
        {
            Console.WriteLine(item.TextContent);
            Console.WriteLine(item.InnerHtml);
            Console.WriteLine(item.OuterHtml);
            Console.WriteLine(item.ToHtml());
        }
    }
}