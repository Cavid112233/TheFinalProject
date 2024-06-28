using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System;
using TheFinalProject.Models;
using TheFinalProject.DAL;
using TheFinalProject.ViewModels.AccountViewModels;
using Microsoft.EntityFrameworkCore;
using TheFinalProject.ViewModels.BasketViewModels;
using Newtonsoft.Json;

namespace TheFinalProject.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly AppDbContext _context;
        public AccountController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<AppUser> signInManager, AppDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _context = context;
        }
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid) return View(registerVM);

            AppUser appUser = new AppUser
            {
                Name = registerVM.Name,
                Surname = registerVM.Surname,
                Email = registerVM.Email,
                UserName = registerVM.Username
            };
            if (!await _userManager.Users.AnyAsync(e => e.NormalizedEmail == registerVM.Email.ToUpperInvariant().Trim()))
            {
                ModelState.AddModelError("Email", $"{registerVM.Email} is already taken");
                return View(registerVM);
            }

            if (!await _userManager.Users.AnyAsync(e => e.NormalizedUserName == registerVM.Username.ToUpperInvariant().Trim()))
            {
                ModelState.AddModelError("Username", $"{registerVM.Username} is already taken");
                return View(registerVM);
            }
            IdentityResult identityResult = await _userManager.CreateAsync(appUser, registerVM.Password);
            if (!identityResult.Succeeded)
            {
                foreach (IdentityError error in identityResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(registerVM);
            }
            await _userManager.AddToRoleAsync(appUser, "Member");
            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        public async Task<IActionResult> CreateRole()
        {
            await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
            await _roleManager.CreateAsync(new IdentityRole("Admin"));
            await _roleManager.CreateAsync(new IdentityRole("Member"));
            return Content("Successfully");

        }

        [HttpGet]
        public async Task<IActionResult> CreateUser()
        {
            AppUser appUser = new AppUser
            {
                Name = "Super",
                Surname = "Admin",
                Email = "superadmin@gmail.com",
                UserName = "SuperAdmin"
            };

            await _userManager.CreateAsync(appUser, "SuperAdmin17");
            await _userManager.AddToRoleAsync(appUser, "SuperAdmin");

            return Content("Role added successfully");

        }

        [HttpGet]
        public async Task<IActionResult> Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (!ModelState.IsValid) return View(loginVM);
            AppUser appUser = await _userManager.Users.Include(u => u.Baskets.Where(b => b.IsDeleted == false))
                .FirstOrDefaultAsync(u => u.NormalizedEmail == loginVM.Email.Trim().ToUpperInvariant());


            if (appUser == null)
            {
                ModelState.AddModelError("", "Email or Password is incorrect ");
                return View(loginVM);

            }

            Microsoft.AspNetCore.Identity.SignInResult signInResult = await _signInManager
                .PasswordSignInAsync(appUser, loginVM.Password, loginVM.RemindMe, true);


            if (signInResult.IsLockedOut)
            {
                ModelState.AddModelError("", $"Your Account has been blocked. It will be active again after {appUser.LockoutEnd} ");
                return View(loginVM);
            }

            if (!signInResult.Succeeded)
            {
                ModelState.AddModelError("", "Email or Password is incorrect ");
                return View(loginVM);
            }
            if ((await _userManager.IsInRoleAsync(appUser, "SuperAdmin")) || (await _userManager.IsInRoleAsync(appUser, "Admin")))
            {
                return RedirectToAction("index", "dashboard", new { area = "Admin" });
            }

            string basket = HttpContext.Request.Cookies["basket"];
            HttpContext.Response.Cookies.Append("basket", "");
            if (appUser.Baskets != null && appUser.Baskets.Count() > 0)
            {
                List<BasketVM> basketVMs = new List<BasketVM>();
                foreach (Basket basket1 in appUser.Baskets)
                {
                    BasketVM basketVM = new BasketVM
                    {
                        Id = (int)basket1.ProductId,
                        Count = basket1.Count
                    };
                    basketVMs.Add(basketVM);
                }
                basket = JsonConvert.SerializeObject(basketVMs);
                HttpContext.Response.Cookies.Append("basket", basket);
            }
            //if (string.IsNullOrWhiteSpace(basket))
            //{


            //}
            //else
            //{
            //    HttpContext.Response.Cookies.Append("basket", "");
            //}


            return RedirectToAction("profile", "Account");
        }
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        [Authorize(Roles = "Member")]
        public async Task<IActionResult> Profile()
        {
            AppUser appUser = await _userManager.Users
                .Include(u => u.Addresses.Where(a => a.IsDeleted == false))
                .Include(o => o.Orders.Where(o => o.IsDeleted == false))
                .ThenInclude(oi => oi.OrderItems.Where(i => i.IsDeleted == false)).ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(u => u.NormalizedUserName == User.Identity.Name.ToUpperInvariant());

            ProfileVM profileVM = new ProfileVM
            {
                Name = appUser.Name,
                Surname = appUser.Surname,
                Username = appUser.UserName,
                Email = appUser.Email,
                Addresses = appUser.Addresses,
                Orders = appUser.Orders
            };

            return View(profileVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Member")]
        public async Task<IActionResult> Profile(ProfileVM profileVM)
        {
            if (!ModelState.IsValid)
            {

                return View(profileVM);
            }
            AppUser appUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (profileVM.Name != null)
            {
                appUser.Name = profileVM.Name;
            }
            if (profileVM.Surname != null)
            {
                appUser.Surname = profileVM.Surname;
            }


            if (appUser.NormalizedEmail != profileVM.Email.Trim().ToUpperInvariant())
            {
                appUser.Email = profileVM.Email;
            }
            if (appUser.NormalizedUserName != profileVM.Username.Trim().ToUpperInvariant())
            {
                appUser.UserName = profileVM.Username;
            }
            appUser = await _userManager.Users
       .Include(u => u.Orders)
       .FirstOrDefaultAsync(u => u.NormalizedUserName == User.Identity.Name.ToUpperInvariant());

            IdentityResult identityResult = await _userManager.UpdateAsync(appUser);
            if (!identityResult.Succeeded)
            {
                foreach (IdentityError identityError in identityResult.Errors)
                {

                    ModelState.AddModelError("", identityError.Description);
                }
                return View(profileVM);
            }
            await _signInManager.SignInAsync(appUser, true);
            if (!string.IsNullOrWhiteSpace(profileVM.OldPassword))
            {
                if (!await _userManager.CheckPasswordAsync(appUser, profileVM.OldPassword))
                {
                    ModelState.AddModelError("OldPassword", "Old Password is incorrect");
                    return View(profileVM);
                }
                if (profileVM.OldPassword == profileVM.NewPassword)
                {
                    ModelState.AddModelError("NewPassword", "Old Password and New Password cannot be similar");
                    return View(profileVM);

                }
                string token = await _userManager.GeneratePasswordResetTokenAsync(appUser);

                identityResult = await _userManager.ResetPasswordAsync(appUser, token, profileVM.NewPassword);

                if (!identityResult.Succeeded)
                {
                    foreach (IdentityError identityError in identityResult.Errors)
                    {

                        ModelState.AddModelError("", identityError.Description);
                    }
                    return View(profileVM);
                }
                await _signInManager.SignOutAsync();
                return RedirectToAction(nameof(Login));
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Member")]
        public async Task<IActionResult> AddAddress(Address address)
        {
            AppUser appUser = await _userManager.Users
                .Include(u => u.Addresses.Where(a => a.IsDeleted == false))
                .FirstOrDefaultAsync(u => u.NormalizedUserName == User.Identity.Name.ToUpperInvariant());

            ProfileVM profileVM = new ProfileVM
            {
                Name = appUser.Name,
                Surname = appUser.Surname,
                Username = appUser.UserName,
                Email = appUser.Email,
                Addresses = appUser.Addresses
            };

            if (!ModelState.IsValid) return View(nameof(Profile), profileVM);
            if (address.IsMain == true && appUser.Addresses != null && appUser.Addresses.Count() > 0 && appUser.Addresses.Any(a => a.IsMain))
            {
                appUser.Addresses.FirstOrDefault(a => a.IsMain).IsMain = false;

            }
            address.UserId = appUser.Id;
            address.CreatedBy = $"{appUser.Name} {appUser.Surname}";
            address.CreatedAt = DateTime.UtcNow.AddHours(4);
            await _context.Addresses.AddAsync(address);
            await _context.SaveChangesAsync();

            TempData["Tab"] = "address";
            return RedirectToAction(nameof(Profile));
        }

    }
}
