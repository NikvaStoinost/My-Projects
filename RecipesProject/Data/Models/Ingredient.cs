namespace RecipesProject.Data.Models
{
    public class Ingredient
    {
        public Ingredient()
        {
            this.Id = Id;
            this.RecipeIngredients = new HashSet<RecipeIngredient>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<RecipeIngredient> RecipeIngredients { get; set; }
    }
}
