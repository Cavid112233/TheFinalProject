using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using TheFinalProject.Areas.Admin.ViewModels.UserViewModels;
using TheFinalProject.DAL;
using TheFinalProject.Models;

namespace TheFinalProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin")]
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserController(UserManager<AppUser> userManager, AppDbContext context, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _context = context;
            _roleManager = roleManager;
        }


        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Index()
        {
            //Not Working
            List<UserVM> users = await _userManager.Users
                .Where(u => u.UserName != User.Identity.Name)
                .Select(x => new UserVM
                {
                    Id = x.Id,
                    Email = x.Email,
                    Name = x.Name,
                    Surname = x.Surname,
                    Username = x.UserName
                })
                .ToListAsync();

            foreach (var item in users)
            {
                string roleId = _context.UserRoles.FirstOrDefault(u => u.UserId == item.Id).RoleId;
                string roleName = _context.Roles.FirstOrDefault(r => r.Id == roleId).Name;
                item.RoleName = roleName;
            }
            return View(users);
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> ChangeRole(string? userId)
        {
            if (string.IsNullOrWhiteSpace(userId)) return BadRequest();
            AppUser appUser = await _userManager.FindByIdAsync(userId);
            if (appUser == null) return NotFound();
            string roleId = _context.UserRoles.FirstOrDefault(u => u.UserId == userId).RoleId;

            UserChangeRole userChangeRole = new UserChangeRole
            {
                UserId = userId,
                RoleId = roleId
            };
            ViewBag.Role = await _roleManager.Roles.Where(r => r.Name != "SuperAdmin").ToListAsync();

            return View(userChangeRole);


        }
        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> ChangeRole(UserChangeRole userChangeRole)
        {
            ViewBag.Role = await _roleManager.Roles.Where(r => r.Name != "SuperAdmin").ToListAsync();
            if (!ModelState.IsValid) return View(userChangeRole);
            if (string.IsNullOrWhiteSpace(userChangeRole.UserId)) return BadRequest();
            AppUser appUser = await _userManager.FindByIdAsync(userChangeRole.UserId);
            if (appUser == null) return NotFound();

            string roleId = _context.UserRoles.FirstOrDefault(u => u.UserId == userChangeRole.UserId).RoleId;
            string roleName = _context.Roles.FirstOrDefault(r => r.Id == roleId).Name;
            string newRolename = _context.Roles.FirstOrDefault(r => r.Name != "SuperAdmin" && r.Id == userChangeRole.RoleId).Name;
            await _userManager.RemoveFromRoleAsync(appUser, roleName);
            await _userManager.AddToRoleAsync(appUser, newRolename);

            return RedirectToAction(nameof(Index));

        }
        public async Task<IActionResult> BlockUser(string userId)
        {

            if (User.IsInRole("SuperAdmin"))
            {
                var user = await _userManager.FindByIdAsync(userId);

                if (user != null)
                {

                    var lockoutEndDate = DateTimeOffset.UtcNow.AddHours(5);
                    user.LockoutEnd = lockoutEndDate;

                    var result = await _userManager.UpdateAsync(user);

                    if (result.Succeeded)
                    {

                        TempData["Message"] = "User blocked successfully.";
                    }
                    else
                    {

                        TempData["ErrorMessage"] = "Failed to block the user.";
                    }
                }
                else
                {

                    TempData["ErrorMessage"] = "User not found.";
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Access denied.";
            }


            return RedirectToAction(nameof(Index));
        }
    }
}
