using Microsoft.AspNetCore.Mvc;

namespace Backend.Areas.AdminArea.Controllers
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
