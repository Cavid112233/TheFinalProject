using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using TheFinalProject.DAL;
using TheFinalProject.Models;

namespace TheFinalProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;

        public CategoryController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            IQueryable<Category> categories = _context.Categories.Where(c => c.IsDeleted == false).OrderByDescending(c => c.Id);
            return View(categories);
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return BadRequest();

            Category category = await _context.Categories.Include(b => b.Products.Where(p => p.IsDeleted == false)).FirstOrDefaultAsync(b => b.IsDeleted == false && b.Id == id);
            if (category == null) return NotFound();
            return View(category);
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Create(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View(category);
            }
            if (await _context.Categories.AnyAsync(b => b.IsDeleted == false && b.Name.ToLower().Contains(category.Name.Trim().ToLower())))
            {
                ModelState.AddModelError("Name", $" {category.Name} category artiq movcuddur.");

                return View(category);
            }
            category.Name = category.Name.Trim();
            category.CreatedBy = "System";
            category.CreatedAt = DateTime.UtcNow.AddHours(4);

            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return BadRequest();

            Category category = await _context.Categories.FirstOrDefaultAsync(b => b.IsDeleted == false && b.Id == id);
            if (category == null) return NotFound();
            return View(category);

        }


        [HttpPost]
        [Authorize(Roles = "SuperAdmin,Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, Category category)
        {
            if (!ModelState.IsValid) return BadRequest();
            if (id == null) return BadRequest();
            if (id != category.Id) return BadRequest();

            Category dbCategory = await _context.Categories.FirstOrDefaultAsync(b => b.IsDeleted == false && b.Id == id);

            if (category == null) return NotFound();
            if (await _context.Categories.AnyAsync(b => b.IsDeleted == false && b.Name.ToLower().Contains(category.Name.Trim().ToLower()) && category.Id != b.Id))
            {
                ModelState.AddModelError("Name", $" {category.Name} category artiq movcudur.");

                return View(category);
            }
            dbCategory.Name = category.Name.Trim();
            dbCategory.UpdatedBy = "System";
            dbCategory.UpdatedAt = DateTime.UtcNow.AddHours(4);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();

            Category category = await _context.Categories.Include(b => b.Products.Where(b => b.IsDeleted == false))
                .FirstOrDefaultAsync(b => b.IsDeleted == false && b.Id == id);

            if (category == null) return NotFound();


            return View(category);
        }


        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> DeleteCategory(int? id)
        {
            if (id == null) return BadRequest();

            Category category = await _context.Categories.Include(b => b.Products.Where(b => b.IsDeleted == false))
                .FirstOrDefaultAsync(b => b.IsDeleted == false && b.Id == id);

            if (category == null) return NotFound();

            category.IsDeleted = true;
            category.DeletedBy = "System";
            category.DeletedAt = DateTime.UtcNow.AddHours(4);

            foreach (Product product in category.Products)
            {
                product.IsDeleted = true;
                product.DeletedBy = "System";
                product.DeletedAt = DateTime.UtcNow.AddHours(4);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }




    }
}
