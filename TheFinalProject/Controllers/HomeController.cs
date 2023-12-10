using Microsoft.AspNetCore.Mvc;

namespace TheFinalProject.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
