namespace RecipesProject.Dto_s
{
    public class RecipeDto
    {
        public RecipeDto()
        {
            this.Ingredients = new List<string>();
        }

        public string CategoryName { get; set; }

        public string RecipeName { get; set; }

        public string Description { get; set; }

        public TimeSpan PreparationTime { get; set; }

        public TimeSpan CookingTime { get; set; }

        public int PortionsCount { get; set; }  

        public string OriginalUrl { get; set; }

        public string Extention { get; set; }

        public List<string> Ingredients { get; set; }

    }
}
