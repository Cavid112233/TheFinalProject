using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using TheFinalProject.DAL;
using TheFinalProject.Models;
using TheFinalProject.ViewModels;
using TheFinalProject.ViewModels.HomeViewModels;

namespace TheFinalProject.Controllers
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
            IEnumerable<Product> products = await _context.Products
                .Include(p => p.Reviews.Where(p => p.IsDeleted == false))
                .Where(s => s.IsDeleted == false).ToListAsync();
            products = products.Take(3);


            HomeVM vm = new HomeVM
            {
                Products = products,
                Settings = await _context.Settings.ToDictionaryAsync(s => s.Key, s => s.Value)

            };

            return View(vm);
        }
    }
}
