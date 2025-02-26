using Microsoft.AspNetCore.Mvc.Rendering;

namespace RecipesProject.Services
{
    public interface ICategoriesService
    {
        IEnumerable<SelectListItem> GetKeyValuePairs();
    }
}
