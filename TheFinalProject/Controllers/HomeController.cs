using Microsoft.AspNetCore.Mvc;
using TheFinalProject.DAL;
using TheFinalProject.ViewModels;

namespace TheFinalProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public HomeController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IActionResult Index()
        {
            HomeVM vm = new()
            {
                ServiceRepairs = _appDbContext.ServiceRepairs.ToList(),
            };

            return View(vm);
        }
    }
}
