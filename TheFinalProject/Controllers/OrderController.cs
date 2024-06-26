using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using TheFinalProject.DAL;
using TheFinalProject.Models;
using TheFinalProject.ViewModels.BasketViewModels;
using TheFinalProject.ViewModels.OrderViewModels;

namespace TheFinalProject.Controllers
{
    [Authorize(Roles = "Member")]
    public class OrderController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public OrderController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        [Authorize(Roles = "Member")]
        public async Task<IActionResult> Checkout()
        {
            string cookie = HttpContext.Request.Cookies["basket"];
            if (string.IsNullOrWhiteSpace(cookie))
            {
                RedirectToAction("Index", "Shop");
            }

            List<BasketVM> basketVMs = JsonConvert.DeserializeObject<List<BasketVM>>(cookie);
            foreach (BasketVM basketVM in basketVMs)
            {
                Product product = await _context.Products.FirstOrDefaultAsync(p => p.Id == basketVM.Id);
                basketVM.Price = product.Price;
                basketVM.Title = product.Title;
                basketVM.DiscountedPrice = product.DiscountedPrice;
                basketVM.Image = product.MainImage;

            }

            AppUser appUser = await _userManager.Users
                .Include(u => u.Addresses.Where(a => a.IsMain && a.IsDeleted == false))
                .FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);

            Order order = new Order
            {
                Name = appUser.Name,
                Surname = appUser.Surname,
                Email = appUser.Email,
                PhoneNumber = appUser.PhoneNumber,
                AddressLine = appUser.Addresses?.FirstOrDefault()?.AddressLine,
                City = appUser.Addresses?.FirstOrDefault()?.City,
                Country = appUser.Addresses?.FirstOrDefault()?.Country,
                State = appUser.Addresses?.FirstOrDefault()?.State,
                PostalCode = appUser.Addresses?.FirstOrDefault()?.PostalCode
            };

            OrderVM orderVM = new OrderVM
            {
                Order = order,
                BasketVMs = basketVMs
            };
            return View(orderVM);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Member")]
        public async Task<IActionResult> Checkout(Order order)
        {
            AppUser appUser = await _userManager.Users
               .Include(u => u.Addresses.Where(a => a.IsMain && a.IsDeleted == false))
               .Include(u => u.Orders)
               .Include(u => u.Baskets.Where(b => b.IsDeleted == false))
               .FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);

            if (appUser.Addresses == null && appUser.Addresses.Count() <= 0)
            {
                ModelState.AddModelError("", "You Need to Add Address First before Ordering");
                return View(order);
            }
            string cookie = HttpContext.Request.Cookies["basket"];
            if (string.IsNullOrWhiteSpace(cookie))
            {
                RedirectToAction("Index", "Shop");
            }

            List<BasketVM> basketVMs = JsonConvert.DeserializeObject<List<BasketVM>>(cookie);
            foreach (BasketVM basketVM in basketVMs)
            {
                Product product = await _context.Products.FirstOrDefaultAsync(p => p.Id == basketVM.Id);
                basketVM.Price = product.Price;
                basketVM.Title = product.Title;
                basketVM.DiscountedPrice = product.DiscountedPrice;
                basketVM.Image = product.MainImage;

            }
            OrderVM orderVM = new OrderVM
            {
                Order = order,
                BasketVMs = basketVMs
            };

            if (!ModelState.IsValid) return View(orderVM);

            List<OrderItem> orderItems = new List<OrderItem>();
            foreach (BasketVM basketVM1 in basketVMs)
            {
                OrderItem orderItem = new OrderItem
                {
                    Count = basketVM1.Count,
                    ProductId = basketVM1.Id,
                    Price = basketVM1.DiscountedPrice > 0 ? basketVM1.DiscountedPrice : basketVM1.Price,
                    CreatedAt = DateTime.UtcNow.AddHours(4),
                    CreatedBy = $"{appUser.Name} {appUser.Surname}"
                };
                orderItems.Add(orderItem);

            };

            List<Basket> basketsToDelete = new List<Basket>();

            foreach (Basket basket in appUser.Baskets)
            {
                basket.IsDeleted = true;
                basketsToDelete.Add(basket);
            }

            foreach (Basket basketToDelete in basketsToDelete)
            {
                appUser.Baskets.Remove(basketToDelete);
                _context.Baskets.Remove(basketToDelete);
            }
            HttpContext.Response.Cookies.Append("basket", "");

            order.UserId = appUser.Id;
            order.CreatedAt = DateTime.UtcNow.AddHours(4);
            order.CreatedBy = $"{appUser.Name} {appUser.Surname}";
            order.OrderItems = orderItems;

            order.No = appUser.Orders != null && appUser.Orders.Count() > 0 ? appUser.Orders.Last().No + 1 : 1;



            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }
    }
}
