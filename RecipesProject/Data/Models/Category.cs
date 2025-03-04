namespace RecipesProject.Data.Models
{
    public class Category
    {
        public Category()
        {
            this.Id = Id;
            this.Recipes = new HashSet<Recipe>();
        }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public bool IsDeleted { get; set; } = false;

        public DateTime? DeletedOn { get; set; }

        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Recipe> Recipes { get; set; }
    }
}
