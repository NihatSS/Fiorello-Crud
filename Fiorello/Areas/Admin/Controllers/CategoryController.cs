using ElearnApp.Data;
using Fiorello.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fiorello.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public CategoryController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<Category> categories = await _context.Categories.OrderByDescending(m => m.Id).ToListAsync();
            return View(categories);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id is null) return BadRequest();

            Category category = await _context.Categories.FirstOrDefaultAsync(m => m.Id == id);

            if (category is null) return NotFound();

            return View(category);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null) return BadRequest();

            var category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);

            if (category == null) return NotFound();
            _context.Categories.Remove(category);
            foreach (var item in _context.Products.Where(x=>x.Category.Name == category.Name))
            {
                _context.Products.Remove(item);
            }
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            bool hasCategory = await _context.Categories.AnyAsync(m => m.Name.Trim() == category.Name.Trim());

            if (hasCategory)
            {
                ModelState.AddModelError("Name", "Category already exist");
                return View();
            }

            await _context.Categories.AddAsync(new Category { Name = category.Name });
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null) return BadRequest();

            Category category = await _context.Categories.FirstOrDefaultAsync(m => m.Id == id);

            if (category is null) return NotFound();

            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, Category category)
        {
            if (id is null) return BadRequest();

            Category existCategory = await _context.Categories.FirstOrDefaultAsync(m => m.Id == id);

            if (existCategory is null) return NotFound();

            if (!ModelState.IsValid)
            {
                return View();
            }

            bool hasCategory = await _context.Categories.AnyAsync(m => m.Name.Trim() == category.Name.Trim() && m.Id != id);

            if (hasCategory)
            {
                ModelState.AddModelError("Name", "Category already exist");
                return View();
            }

            existCategory.Name = category.Name;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
