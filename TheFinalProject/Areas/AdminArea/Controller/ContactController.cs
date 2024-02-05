using Microsoft.AspNetCore.Mvc;

namespace TheFinalProject.Areas.AdminArea.Controller
{
    [Area("AdminArea")]
    public class ContactController : Controllers
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
