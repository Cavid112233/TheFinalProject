using Microsoft.AspNetCore.Mvc;
using TheFinalProject.DAL;
using TheFinalProject.Entities;
using TheFinalProject.ViewModels;

namespace TheFinalProject.Controllers
{
    public class BlogController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public BlogController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IActionResult Index()
        {
            BlogVM vm = new()
            {
                BlogPosts = _appDbContext.BlogPosts.ToList(),
               
            };

            return View(vm);
        }
    }
}
