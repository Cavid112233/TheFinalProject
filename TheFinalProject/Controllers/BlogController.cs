using Microsoft.AspNetCore.Mvc;

namespace TheFinalProject.Controllers
{
    public class BlogController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
