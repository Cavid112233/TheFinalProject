using Microsoft.AspNetCore.Mvc;
using TheFinalProject.DAL;
using TheFinalProject.Entities;
using TheFinalProject.ViewModels;

namespace TheFinalProject.Controllers
{
    public class AboutController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public AboutController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public IActionResult Index()
        {
            AboutVM vm = new()
            {
                FastestRepairServiceMains = _appDbContext.FastestRepairServiceMains.ToList(),
                ServiceAbouts = _appDbContext.ServiceAbouts.ToList(),
                ExperiencedStaffs = _appDbContext.ExperiencedStaffs.ToList(),
            };

            return View(vm);
        }
    }
}
