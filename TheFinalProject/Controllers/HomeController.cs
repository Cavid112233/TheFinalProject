using Microsoft.AspNetCore.Mvc;
using TheFinalProject.DAL;
using TheFinalProject.Entities;
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
                Services2s = _appDbContext.Services2s.ToList(),
                ServiceProcesss = _appDbContext.ServiceProcesss.ToList(),
                Productss = _appDbContext.Productss.ToList(),
                Feedbacks = _appDbContext.Feedbacks.ToList(),
                LatestBlogs = _appDbContext.LatestBlogs.ToList(),

            };

            return View(vm);
        }
    }
}
