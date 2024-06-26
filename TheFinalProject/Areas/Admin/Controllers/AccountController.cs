using Microsoft.AspNetCore.Mvc;

namespace TheFinalProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountController : Controller
    {

        public async Task<IActionResult> Register()
        {

            return View();
        }
    }
}
