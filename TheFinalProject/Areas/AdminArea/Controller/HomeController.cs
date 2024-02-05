using Microsoft.AspNetCore.Mvc;

namespace TheFinalProject.Areas.AdminArea.Controller
{
    [Area("AdminArea")]
    public class HomeController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }
    }
}
