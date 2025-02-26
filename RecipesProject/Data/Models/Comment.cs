namespace RecipesProject.Data.Models
{
    public class Comment
    {
        public Comment()
        {
            this.Id = Id;
        }

        public int Id { get; set; }

        public string Content { get; set; }

        public DateTime CreatedAt { get; set; }

        public int RecipeId { get; set; }

        public virtual Recipe Recipe { get; set; }
    }
}
