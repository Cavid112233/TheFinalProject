using Microsoft.AspNetCore.Mvc;

namespace TheFinalProject.Controllers
{
    public class ShopController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
