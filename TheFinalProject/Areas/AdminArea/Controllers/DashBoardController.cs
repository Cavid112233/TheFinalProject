using Microsoft.AspNetCore.Mvc;

namespace TheFinalProject.Areas.AdminArea.Controllers
{
    public class DashBoardController : Controller
    {
        [Area("AdminArea")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
