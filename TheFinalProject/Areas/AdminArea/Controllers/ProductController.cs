using Microsoft.AspNetCore.Mvc;

namespace TheFinalProject.Areas.AdminArea.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
