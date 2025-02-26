namespace RecipesProject.Data.Models
{
    public class Category
    {
        public Category()
        {
            this.Id = Id;
            this.Recipes = new HashSet<Recipe>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Recipe> Recipes { get; set; }
    }
}
