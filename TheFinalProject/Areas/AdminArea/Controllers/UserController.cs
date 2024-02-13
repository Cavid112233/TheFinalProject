using TheFinalProject.Entities;
using TheFinalProject.ViewModels.UserAdminn;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace TheFinalProject.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        public UserController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult Index(string search)
        {

            var users = search == null ? _userManager.Users.ToList() : _userManager.Users.Where(u => u.FullName.
            Contains(search)).ToList();


            return View(users);
        }
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();
            await _userManager.DeleteAsync(user);
            return RedirectToAction("index");

        }

        public async Task<IActionResult> Update(string id)

        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            UpdateUserVM updateUserVM = new UpdateUserVM();
            user.Id = updateUserVM.Id;
            user.FullName = updateUserVM.FullName;
            user.UserName = updateUserVM.UserName;
            user.Email = updateUserVM.Email;
            return View(updateUserVM);
        }
        [HttpPost]
        public async Task<IActionResult> Update(UpdateUserVM updateUserVM)
        {
            var user = await _userManager.FindByIdAsync(updateUserVM.Id);
            if (user == null)
            {
                return NotFound();
            }
            else
            {
                user.FullName = updateUserVM.FullName;
                user.UserName = updateUserVM.UserName;
                user.Email = updateUserVM.Email;

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction("index");
                }

                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
                return View(updateUserVM);
            }

        }

    }
}
