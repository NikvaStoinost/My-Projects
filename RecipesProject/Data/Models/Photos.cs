namespace RecipesProject.Data.Models
{
    public class Photos
    {
        public Photos()
        {
            this.Id = Id;
        }

        public int Id { get; set; }

        public string Extension { get; set; }

        public int RecipeId { get; set; }

        public virtual Recipe Recipe { get; set; }

        public string? UserId { get; set; }

        public CustomApplicationUser User { get; set; }
    }
}
