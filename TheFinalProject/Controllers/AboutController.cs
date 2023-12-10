using Microsoft.AspNetCore.Mvc;

namespace TheFinalProject.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
