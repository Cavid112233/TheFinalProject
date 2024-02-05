using Microsoft.AspNetCore.Mvc;
using TheFinalProject.DAL;
using TheFinalProject.ViewModels;

namespace TheFinalProject.Controllers
{
    public class ContactController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public ContactController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public IActionResult Index()
        {
            ContactVM vm = new()
            {
                GetInTouches = _appDbContext.GetInTouches.ToList()
            };
            return View(vm);
        }
    }
}
