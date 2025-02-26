using Microsoft.AspNetCore.Mvc.Rendering;
using RecipesProject.Data;

namespace RecipesProject.Services
{
    public class CategoriesService : ICategoriesService
    {
        private readonly ApplicationDbContext db;

        public CategoriesService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public IEnumerable<SelectListItem> GetKeyValuePairs()
        {
            return db.Categories
          .Select(x => new SelectListItem
          {
              Value = x.Id.ToString(),
              Text = x.Name
          })
          .ToList();
        }
    }
}
