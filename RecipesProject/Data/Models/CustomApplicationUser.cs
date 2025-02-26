using Microsoft.AspNetCore.Identity;

namespace RecipesProject.Data.Models
{
    public class CustomApplicationUser : IdentityUser
    {
        public CustomApplicationUser()
        {
            this.Recipes = new HashSet<Recipe>();
        }

        public virtual ICollection<Recipe> Recipes { get; set; } 
    }
}
