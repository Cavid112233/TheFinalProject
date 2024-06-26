using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using TheFinalProject.DAL;
using TheFinalProject.Extensions;
using TheFinalProject.Helpers;
using TheFinalProject.Models;
using TheFinalProject.ViewModels;

namespace TheFinalProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ProductController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        [Authorize(Roles = "SuperAdmin,Admin")]
        public IActionResult Index(int pageIndex = 1)
        {
            IEnumerable<Product> products = _context.Products
                .Where(p => p.IsDeleted == false);
            return View(PagenatedList<Product>.Create(products, pageIndex, 4));

        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return BadRequest();

            Product product = await _context.Products.Include(pi => pi.ProductImages.Where(pii => pii.IsDeleted == false))
                .FirstOrDefaultAsync(p => p.Id == id && p.IsDeleted == false);

            if (product == null) return NotFound();

            return View(product);
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Create()
        {
            ViewBag.Materials = await _context.Materials.Where(b => b.IsDeleted == false).ToListAsync();
            ViewBag.Categories = await _context.Categories.Where(b => b.IsDeleted == false).ToListAsync();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Create(Product product)
        {
            ViewBag.Materials = await _context.Materials.Where(b => b.IsDeleted == false).ToListAsync();
            ViewBag.Categories = await _context.Categories.Where(b => b.IsDeleted == false).ToListAsync();

            if (!ModelState.IsValid) return View(product);
            if (!await _context.Materials.AnyAsync(m => m.IsDeleted == false && m.Id == product.MaterialId))
            {
                ModelState.AddModelError("MaterialId", $"Daxil olunan Material Id {product.MaterialId} yanlishdir");
            }
            if (!await _context.Categories.AnyAsync(c => c.IsDeleted == false && c.Id == product.CategoryId))
            {
                ModelState.AddModelError("CategoryId", $"Daxil olunan Category Id {product.CategoryId} yanlishdir");
            }

            if (product.MainFile != null)
            {
                if (!(product.MainFile.CheckFileContentType("image/jpeg") || product.MainFile.CheckFileContentType("image/png")))
                {
                    ModelState.AddModelError("MainFile", "Main File Jpg ve ya Png olmalidir");
                    return View(product);
                }
                if (!product.MainFile.CheckFileLength(3500))
                {

                    ModelState.AddModelError("MainFile", "Main File 3.5mb-dan kicik olmalidir");
                    return View(product);
                }
                product.MainImage = await product.MainFile.CreateFileAsync(_env, "assets", "images", "product");

            }
            else
            {
                ModelState.AddModelError("MainFile", "File Mutleq Daxil Olmalidir");
                return View(product);
            }

            if (product.Files == null)
            {
                ModelState.AddModelError("Files", "Wekil mutleq secilmelidir");
                return View(product);
            }

            if (product.Files.Count() > 5)
            {
                ModelState.AddModelError("Files", "Max 5 wekil ola biler");
                return View(product);

            }

            if (product.Files.Count() > 0)
            {
                List<ProductImage> productImages = new List<ProductImage>();
                foreach (IFormFile file in product.Files)
                {
                    if (!(file.CheckFileContentType("image/jpeg") || file.CheckFileContentType("image/png")))
                    {
                        ModelState.AddModelError("Files", $"{file.FileName} Jpg ve ya png olmalidir");
                        return View(product);
                    }
                    if (!file.CheckFileLength(3000))
                    {

                        ModelState.AddModelError("Files", $"{file.FileName} 3000kb olmalidir");
                        return View(product);
                    }
                    ProductImage productImage = new ProductImage
                    {

                        Image = await file.CreateFileAsync(_env, "assets", "images", "product"),
                        CreatedAt = DateTime.UtcNow.AddHours(4),
                        CreatedBy = "System"

                    };
                    productImages.Add(productImage);
                }
                product.ProductImages = productImages;
            }
            product.Title = product.Title.Trim();
            product.CreatedBy = "System";
            product.CreatedAt = DateTime.UtcNow.AddHours(4);
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }
        [HttpGet]
        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            Product product = await _context.Products
                .Include(p => p.ProductImages.Where(pi => pi.IsDeleted == false))
                .FirstOrDefaultAsync(p => p.Id == id && p.IsDeleted == false);

            if (product == null)
            {
                return NotFound();
            }

            ViewBag.Materials = await _context.Materials.Where(b => b.IsDeleted == false).ToListAsync();
            ViewBag.Categories = await _context.Categories.Where(b => b.IsDeleted == false).ToListAsync();

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<IActionResult> Update(int? id, Product product)
        {
            ViewBag.Materials = await _context.Materials.Where(b => b.IsDeleted == false).ToListAsync();
            ViewBag.Categories = await _context.Categories.Where(b => b.IsDeleted == false).ToListAsync();

            if (!ModelState.IsValid)
            {
                return View(product);
            }

            if (id == null) return BadRequest();

            if (id != product.Id) return BadRequest();

            Product dbProduct = await _context.Products
            .Include(pt => pt.ProductImages.Where(p => p.IsDeleted == false))
            .FirstOrDefaultAsync(p => p.IsDeleted == false && p.Id == id);

            if (dbProduct == null) return NotFound();
            int canUpload = 5 - dbProduct.ProductImages.Count();
            if (product.Files != null && canUpload < product.Files.Count())
            {
                ModelState.AddModelError("Files", $"Maksimum {canUpload} qeder şekil yükleye bilersiniz");
                return View(product);
            }
            if (product.Files != null && product.Files.Count() > 0)
            {
                List<ProductImage> productImages = new List<ProductImage>();

                foreach (IFormFile file in product.Files)
                {
                    if (!file.CheckFileContentType("image/jpeg"))
                    {
                        ModelState.AddModelError("Files", $"{file.FileName} jpeg tipinde olmalidir!");
                        return View(product);
                    }
                    if (!file.CheckFileLength(1800))
                    {
                        ModelState.AddModelError("Files", $"{file.FileName} Main File 1800 KB dan cox ola bilmez!");
                        return View(product);
                    }
                    ProductImage productImage = new ProductImage
                    {
                        Image = await file.CreateFileAsync(_env, "assets", "images", "product"),
                        CreatedAt = DateTime.UtcNow.AddHours(4),
                        CreatedBy = "System"
                    };

                    productImages.Add(productImage);

                }

                dbProduct.ProductImages.AddRange(productImages);
            }
            if (product.MainFile != null)
            {
                if (!product.MainFile.CheckFileContentType("image/jpeg"))
                {
                    ModelState.AddModelError("MainFile", "Main File jpeg tipinde olmalidir!");
                    return View(product);
                }
                if (!product.MainFile.CheckFileLength(1800))
                {
                    ModelState.AddModelError("MainFile", "Main File 1.8 MB-dan cox ola bilmez!");
                    return View(product);
                }
                FileHelper.DeleteFile(dbProduct.MainImage, _env, "assets", "images", "product");
                dbProduct.MainImage = await product.MainFile.CreateFileAsync(_env, "assets", "images", "product");
            }
            //dbProduct.Count = product.Count;
            dbProduct.Title = product.Title;
            dbProduct.Description = product.Description;
            dbProduct.FullDescription = product.FullDescription;
            dbProduct.Price = product.Price;
            dbProduct.DiscountedPrice = product.DiscountedPrice;
            dbProduct.Count = product.Count;
            dbProduct.UpdatedBy = "Admin";
            dbProduct.UpdatedAt = DateTime.UtcNow.AddHours(4);


            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<IActionResult> DeleteImageOfProduct(int? id, int? imageId)
        {
            if (id == null) return BadRequest();
            if (imageId == null) return BadRequest();
            Product product = await _context.Products
                .Include(p => p.ProductImages.Where(p => p.IsDeleted == false))
                .FirstOrDefaultAsync(p => p.IsDeleted == false && p.Id == id);

            if (product == null) return NotFound();
            if (product.ProductImages?.Count() <= 1) return BadRequest();

            if (!product.ProductImages.Any(p => p.Id == imageId)) return BadRequest();
            product.ProductImages.FirstOrDefault(p => p.Id == imageId).IsDeleted = true;

            await _context.SaveChangesAsync();
            FileHelper.DeleteFile(product.ProductImages.FirstOrDefault(p => p.Id == imageId).Image, _env, "assets", "images", "product");
            List<ProductImage> productImages = product.ProductImages.Where(p => p.IsDeleted == false).ToList();
            return PartialView("_ProductImagesPartial", productImages);
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();

            Product product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id && p.IsDeleted == false);

            if (product == null) return NotFound();

            return View(product);
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> DeleteProduct(int? id)
        {
            if (id == null) return BadRequest();

            Product product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id && p.IsDeleted == false);

            if (product == null) return NotFound();

            product.IsDeleted = true;
            product.DeletedAt = DateTime.UtcNow.AddHours(4);
            product.DeletedBy = "System";

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }
    }

}
