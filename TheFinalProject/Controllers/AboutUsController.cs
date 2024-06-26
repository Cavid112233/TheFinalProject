using Microsoft.AspNetCore.Mvc;

namespace TheFinalProject.Controllers
{
    public class AboutUsController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ContactUs()
        {
            return View();
        }
    }
}
