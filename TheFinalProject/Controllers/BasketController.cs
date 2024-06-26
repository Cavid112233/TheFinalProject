using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using TheFinalProject.DAL;
using TheFinalProject.Models;
using TheFinalProject.ViewModels.BasketViewModels;

namespace TheFinalProject.Controllers
{
    public class BasketController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public BasketController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            string basket = HttpContext.Request.Cookies["basket"];
            List<BasketVM> basketVMs = null;
            if (basket != null)
            {
                basketVMs = JsonConvert.DeserializeObject<List<BasketVM>>(basket);
            }
            else
            {
                basketVMs = new List<BasketVM>();
            }
            foreach (BasketVM basketVM in basketVMs)
            {
                basketVM.Title = _context.Products.FirstOrDefault(p => p.Id == basketVM.Id).Title;
                basketVM.Image = _context.Products.FirstOrDefault(p => p.Id == basketVM.Id).MainImage;
                basketVM.Price = _context.Products.FirstOrDefault(p => p.Id == basketVM.Id).Price;
                basketVM.DiscountedPrice = _context.Products.FirstOrDefault(p => p.Id == basketVM.Id).DiscountedPrice;
            }
            return View(basketVMs);

        }
        [AllowAnonymous]
        //[Authorize(Roles = "Member")]
        public async Task<IActionResult> AddToBasket(int? id)
        {
            if (id == null) { return BadRequest(); }

            if (!await _context.Products.AnyAsync(p => p.IsDeleted == false && p.Id == id)) { return NotFound(); }

            string basket = HttpContext.Request.Cookies["basket"];

            List<BasketVM> basketVMs = null;

            if (string.IsNullOrWhiteSpace(basket))
            {
                basketVMs = new List<BasketVM>
                {
                    new BasketVM {Id = (int)id,Count = 1}
                };
            }
            else
            {
                basketVMs = JsonConvert.DeserializeObject<List<BasketVM>>(basket);

                if (basketVMs.Exists(b => b.Id == id))
                {
                    basketVMs.Find(b => b.Id == id).Count += 1;
                }
                else
                {
                    basketVMs.Add(new BasketVM { Id = (int)id, Count = 1 });
                }
            }
            if (User.Identity.IsAuthenticated)
            {
                AppUser appUser = await _userManager.Users
                    .Include(u => u.Baskets.Where(b => b.IsDeleted == false))
                .FirstOrDefaultAsync(u => u.NormalizedUserName == User.Identity.Name.ToUpperInvariant());

                if (appUser.Baskets.Any(b => b.ProductId == id))
                {
                    appUser.Baskets.FirstOrDefault(b => b.ProductId == id).Count = basketVMs.FirstOrDefault(b => b.Id == id).Count;
                }
                else
                {
                    Basket dbbasket = new Basket
                    {
                        ProductId = id,
                        Count = basketVMs.FirstOrDefault(b => b.Id == id).Count
                    };
                    appUser.Baskets.Add(dbbasket);
                }
                await _context.SaveChangesAsync();

            }

            basket = JsonConvert.SerializeObject(basketVMs);

            HttpContext.Response.Cookies.Append("basket", basket);
            foreach (BasketVM basketVM in basketVMs)
            {
                Product product = await _context.Products
                    .FirstOrDefaultAsync(p => p.Id == basketVM.Id && p.IsDeleted == false);

                if (product != null)
                {
                    basketVM.Price = product.Price;
                    basketVM.DiscountedPrice = product.DiscountedPrice;
                    basketVM.Title = product.Title;
                    basketVM.Image = product.MainImage;

                }
            }

            return PartialView("_BasketPartial", basketVMs);

        }


        public async Task<IActionResult> GetBasket()
        {
            return Json(JsonConvert.DeserializeObject<List<BasketVM>>(HttpContext.Request.Cookies["basket"]));

        }


        [HttpGet]
        [AllowAnonymous]
        //[Authorize(Roles = "Member")]
        public async Task<IActionResult> DeleteFromBasket(int? id)
        {
            if (id == null) return BadRequest();
            if (!await _context.Products.AnyAsync(p => p.Id == id)) return NotFound();

            string basket = HttpContext.Request.Cookies["basket"];

            if (string.IsNullOrWhiteSpace(basket))
            {
                return BadRequest();
            }

            List<BasketVM> basketVMs = JsonConvert.DeserializeObject<List<BasketVM>>(basket);
            BasketVM basketVM = basketVMs.Find(b => b.Id == id);

            if (basketVM == null) return NotFound();

            if (User.Identity.IsAuthenticated)
            {
                AppUser appUser = await _userManager.Users
                    .Include(u => u.Baskets)
                    .FirstOrDefaultAsync(u => u.NormalizedUserName == User.Identity.Name.ToUpperInvariant());

                Basket basketToDelete = appUser.Baskets.FirstOrDefault(b => b.ProductId == id);
                if (basketToDelete != null)
                {
                    appUser.Baskets.Remove(basketToDelete);
                    _context.Baskets.Remove(basketToDelete);
                    await _context.SaveChangesAsync();
                }
            }

            basketVMs.Remove(basketVM);
            foreach (var item in basketVMs)
            {
                var product = await _context.Products.FindAsync(item.Id);
                item.Title = product.Title;
                item.Image = product.MainImage;
                item.Price = product.Price;
                item.DiscountedPrice = product.DiscountedPrice;
            }

            basket = JsonConvert.SerializeObject(basketVMs);
            HttpContext.Response.Cookies.Append("basket", basket);

            return PartialView("_BasketPartial", basketVMs);
        }

        [HttpGet]
        [AllowAnonymous]
        //[Authorize(Roles = "Member")]
        public async Task<IActionResult> RefreshBasket()
        {
            string basket = HttpContext.Request.Cookies["basket"];

            if (string.IsNullOrWhiteSpace(basket))
            {
                return BadRequest();
            }

            List<BasketVM> basketVMs = JsonConvert.DeserializeObject<List<BasketVM>>(basket);

            foreach (var item in basketVMs)
            {
                var product = await _context.Products.FindAsync(item.Id);
                item.Title = product.Title;
                item.Image = product.MainImage;
                item.Price = product.Price;
                item.DiscountedPrice = product.DiscountedPrice;
            }

            return PartialView("_BasketPartial", basketVMs);
        }

        [HttpGet]
        [AllowAnonymous]
        //[Authorize(Roles = "Member")]
        public async Task<IActionResult> RefreshIndex()
        {
            string basket = HttpContext.Request.Cookies["basket"];

            if (string.IsNullOrWhiteSpace(basket))
            {
                return BadRequest();
            }

            List<BasketVM> basketVMs = JsonConvert.DeserializeObject<List<BasketVM>>(basket);

            foreach (var item in basketVMs)
            {
                var product = await _context.Products.FindAsync(item.Id);
                item.Title = product.Title;
                item.Image = product.MainImage;
                item.Price = product.Price;
                item.DiscountedPrice = product.DiscountedPrice;

            }

            return PartialView("_BasketProductTablePartial", basketVMs);
        }


        [HttpGet]
        [AllowAnonymous]
        //[Authorize(Roles = "Member")]
        public async Task<IActionResult> DeleteFromCart(int? id)
        {
            if (id == null) return BadRequest();
            if (!await _context.Products.AnyAsync(p => p.Id == id)) return NotFound();

            string basket = HttpContext.Request.Cookies["basket"];

            if (string.IsNullOrWhiteSpace(basket))
            {
                return BadRequest();
            }

            List<BasketVM> basketVMs = JsonConvert.DeserializeObject<List<BasketVM>>(basket);
            BasketVM basketVM = basketVMs.Find(b => b.Id == id);

            if (basketVM == null) return NotFound();
            if (User.Identity.IsAuthenticated)
            {
                AppUser appUser = await _userManager.Users
                    .Include(u => u.Baskets)
                    .FirstOrDefaultAsync(u => u.NormalizedUserName == User.Identity.Name.ToUpperInvariant());

                Basket basketToDelete = appUser.Baskets.FirstOrDefault(b => b.ProductId == id);
                if (basketToDelete != null)
                {
                    appUser.Baskets.Remove(basketToDelete);
                    await _context.SaveChangesAsync();
                }
            }

            basketVMs.Remove(basketVM);
            foreach (var item in basketVMs)
            {
                var product = await _context.Products.FindAsync(item.Id);
                item.Title = product.Title;
                item.Image = product.MainImage;
                item.Price = product.Price;
                item.DiscountedPrice = product.DiscountedPrice;

            }

            basket = JsonConvert.SerializeObject(basketVMs);
            HttpContext.Response.Cookies.Append("basket", basket);

            return PartialView("_BasketProductTablePartial", basketVMs);

        }


    }
}
