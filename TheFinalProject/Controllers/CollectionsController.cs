using Microsoft.AspNetCore.Mvc;

namespace TheFinalProject.Controllers
{
    public class CollectionsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
