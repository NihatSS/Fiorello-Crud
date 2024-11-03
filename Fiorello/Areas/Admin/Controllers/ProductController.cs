using ElearnApp.Data;
using Fiorello.Models;
using Fiorello.ViewModels.Admin.Product;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fiorello.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ProductController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.Products.Include(x=>x.Images).Include(x=>x.Category).OrderByDescending(x=>x.Id).ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id is null) return BadRequest();

            var work = await _context.Products.Include(x=>x.Images).Include(x=>x.Category).FirstOrDefaultAsync(x=>x.Id == id);

            if (work is null) return NotFound();

            return View(work);
        }

        public async Task<IActionResult> Create()
        {

            return View(new ProductCreateVM { Categories = await _context.Categories.ToListAsync(), Product = await _context.Products.FirstOrDefaultAsync()});
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductCreateVM request)
        {
            if (!ModelState.IsValid) return View(request);

            var product = new Product
            {
                Name = request.Product.Name,
                Description = request.Product.Description,
                Price = request.Product.Price,
                CategoryId = request.Product.CategoryId,
                Images = new List<ProductImage>()
            };

            if (request.Product.UploadedImages != null && request.Product.UploadedImages.Count > 0)
            {
                for (int i = 0; i < request.Product.UploadedImages.Count; i++)
                {
                    var item = request.Product.UploadedImages[i];
                    string fileName = $"{Guid.NewGuid()}_{item.FileName}";
                    string path = Path.Combine(_env.WebRootPath, "assets/img", fileName);

                    using (FileStream stream = new(path, FileMode.Create))
                    {
                        await item.CopyToAsync(stream);
                    }


                    var workImage = new ProductImage
                    {
                        Image = fileName,
                        Product = product,
                        IsMain = (i == 0)
                    };

                    product.Images.Add(workImage);
                }
            }

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null) return BadRequest();

            var work = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);

            if (work == null) return NotFound();
            _context.Products.Remove(work);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return BadRequest();

            var productEditVM = new ProductEditVM
            {
                Product = await _context.Products.Include(x=>x.Images)
                                                 .Include(x=>x.Category)
                                                 .FirstOrDefaultAsync(x=>x.Id == id),
                                                 
                Categories = await _context.Categories.ToListAsync(),
            };

            if (productEditVM == null) return NotFound();

            return View(productEditVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, ProductEditVM productEditVM)
        {
            if (id == null) return NotFound();

            var findData = await _context.Products
                .Include(p => p.Images)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (findData == null) return NotFound();

            findData.Name = productEditVM.Product.Name;
            findData.Description = productEditVM.Product.Description;
            findData.CategoryId = productEditVM.Product.CategoryId;

            if (productEditVM.Product.Category != null)
            {
                findData.Category.Name = productEditVM.Product.Category.Name;
            }

            if (findData.Images != null && findData.Images.Count > 0)
            {
                foreach (var image in findData.Images.ToList())
                {
                    var oldImagePath = Path.Combine(_env.WebRootPath, "assets/img", image.Image);
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                    _context.ProductImages.Remove(image);
                }
            }
            bool mainImage = true;

            if (productEditVM.Product.UploadedImages != null && productEditVM.Product.UploadedImages.Count > 0)
            {
                foreach (var item in productEditVM.Product.UploadedImages)
                {
                    string fileName = $"{Guid.NewGuid()}_{item.FileName}";
                    string path = Path.Combine(_env.WebRootPath, "assets/img", fileName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await item.CopyToAsync(stream);
                    }

                    findData.Images.Add(new ProductImage
                    {
                        Image = fileName,
                        IsMain = mainImage,
                        ProductId = findData.Id
                    });

                    mainImage = false;
                }

            }

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }


    }
}
