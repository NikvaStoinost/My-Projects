using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipesProject.Data;

namespace RecipesProject.Areas.Identity
{
    [Area("Identity")]
    [Authorize(Roles ="Admin")]
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext _db;

        public CategoriesController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET: Identity/Categories
        public async Task<IActionResult> Index()
        {
            return View(await _db.Categories.ToListAsync());
        }

        // GET: Identity/Categories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _db.Categories
                .FirstOrDefaultAsync(m => m.Id == id);

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Identity/Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _db.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            category.IsDeleted = true;
            category.DeletedOn = DateTime.UtcNow;

            await _db.SaveChangesAsync();

            return RedirectToAction("Index", "Categories", new { area = "Identity" });
        }
    }
}
