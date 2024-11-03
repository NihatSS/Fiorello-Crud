using ElearnApp.Data;
using Fiorello.Services.Interfaces;
using Fiorello.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fiorello.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            HomeVM model = new()
            {
                Slider = await _context.Sliders.FirstOrDefaultAsync(),
                SliderImages = await _context.SliderImages.ToListAsync(),
                Products = await _context.Products.Include(m=>m.Images).ToListAsync(),
                Categories = await _context.Categories.ToListAsync(),
            };
            return View(model);
        }
    }
}
