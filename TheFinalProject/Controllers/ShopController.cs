using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using TheFinalProject.DAL;
using TheFinalProject.Models;
using TheFinalProject.ViewModels.ShopViewModels;

namespace TheFinalProject.Controllers
{
    public class ShopController : Controller
    {
        private readonly AppDbContext _context;

        public ShopController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(int? categoryId, int pageIndex = 1)
        {
            IEnumerable<Product> products = _context.Products
                .Where(p => p.IsDeleted == false);

            ViewBag.totalPage = (int)Math.Ceiling((decimal)products.Count() / 6);

            products = products.Skip((pageIndex - 1) * 6).Take(6);

            ViewBag.pageIndex = pageIndex;


            ViewBag.categoryId = categoryId;
            ShopVM shopVM = new ShopVM
            {
                Products = products,
                Materials = await _context.Materials.Where(m => m.IsDeleted == false).ToListAsync(),
                Categories = await _context.Categories.Where(c => c.IsDeleted == false).ToListAsync()
            };
            return View(shopVM);
        }

        public async Task<IActionResult> ShopFilters(int? categoryId, int? materialId, double? minPrice, double? maxPrice, int pageIndex = 1)
        {
            IEnumerable<Product> products = _context.Products
                .Where(p => p.IsDeleted == false);
            if (categoryId != null)
            {
                products = products.Where(p => p.CategoryId == categoryId);
                ViewBag.categoryId = categoryId;
            }
            if (materialId != null)
            {
                products = products.Where(p => p.MaterialId == materialId);
                ViewBag.materialId = materialId;
            }
            if (minPrice >= 0 && maxPrice > 0)
            {
                products = products.Where(p => p.IsDeleted == false && (p.DiscountedPrice > 0 ?
                    p.DiscountedPrice >= minPrice && p.DiscountedPrice <= (minPrice == 0 ? 8500 : maxPrice) :
                    p.Price >= minPrice && p.Price <= (maxPrice == 0 ? 8500 : maxPrice)));
                ViewBag.minPrice = minPrice;
                ViewBag.maxPrice = maxPrice;
            }

            ViewBag.totalPage = (int)Math.Ceiling((decimal)products.Count() / 6);
            products = products.Skip((pageIndex - 1) * 6).Take(6);
            ViewBag.pageIndex = pageIndex;

            ShopVM shopVM = new ShopVM
            {
                Products = products,
                Materials = await _context.Materials.Where(m => m.IsDeleted == false).ToListAsync(),
                Categories = await _context.Categories.Where(c => c.IsDeleted == false).ToListAsync()
            };

            return PartialView("_ShopListPartial", shopVM);
        }
    }
}
