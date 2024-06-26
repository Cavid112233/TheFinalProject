using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace TheFinalProject.Areas.Admin.Controllers
{
    [Area("manage")]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class MaterialController : Controller
    {
        private readonly AppDbContext _context;

        public MaterialController(AppDbContext context)
        {
            _context = context;
        }
        [Authorize(Roles = "SuperAdmin,Admin")]
        public IActionResult Index()
        {
            IQueryable<Material> materials = _context.Materials.Where(m => m.IsDeleted == false).OrderByDescending(m => m.Id);
            return View(materials);
        }
        [HttpGet]
        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return BadRequest();

            Material material = await _context.Materials.Include(b => b.Products
            .Where(p => p.IsDeleted == false))
            .FirstOrDefaultAsync(b => b.IsDeleted == false && b.Id == id);
            if (material == null) return NotFound();
            return View(material);
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
        public async Task<IActionResult> Create(Material material)
        {
            if (!ModelState.IsValid)
            {
                return View(material);
            }
            if (await _context.Materials.AnyAsync(b => b.IsDeleted == false && b.Name.ToLower().Contains(material.Name.Trim().ToLower())))
            {
                ModelState.AddModelError("Name", $" {material.Name} material artiq movcuddur.");

                return View(material);
            }
            material.Name = material.Name.Trim();
            material.CreatedBy = "System";
            material.CreatedAt = DateTime.UtcNow.AddHours(4);

            await _context.Materials.AddAsync(material);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return BadRequest();

            Material material = await _context.Materials.FirstOrDefaultAsync(b => b.IsDeleted == false && b.Id == id);
            if (material == null) return NotFound();
            return View(material);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<IActionResult> Update(int? id, Material material)
        {
            if (!ModelState.IsValid) return BadRequest();
            if (id == null) return BadRequest();
            if (id != material.Id) return BadRequest();

            Material dbMaterial = await _context.Materials.FirstOrDefaultAsync(b => b.IsDeleted == false && b.Id == id);

            if (material == null) return NotFound();
            if (await _context.Materials.AnyAsync(b => b.IsDeleted == false && b.Name.ToLower().Contains(material.Name.Trim().ToLower()) && material.Id != b.Id))
            {
                ModelState.AddModelError("Name", $" {material.Name} material artiq movcuddur.");

                return View(material);
            }
            dbMaterial.Name = material.Name.Trim();
            dbMaterial.UpdatedBy = "System";
            dbMaterial.UpdatedAt = DateTime.UtcNow.AddHours(4);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();

            Material material = await _context.Materials.Include(b => b.Products.Where(b => b.IsDeleted == false))
                .FirstOrDefaultAsync(b => b.IsDeleted == false && b.Id == id);

            if (material == null) return NotFound();

            return View(material);
        }


        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> DeleteMaterial(int? id)
        {
            if (id == null) return BadRequest();

            Material material = await _context.Materials.Include(b => b.Products.Where(b => b.IsDeleted == false))
                .FirstOrDefaultAsync(b => b.IsDeleted == false && b.Id == id);

            if (material == null) return NotFound();

            material.IsDeleted = true;
            material.DeletedBy = "System";
            material.DeletedAt = DateTime.UtcNow.AddHours(4);

            foreach (Product product in material.Products)
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
