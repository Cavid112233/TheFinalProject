using Microsoft.AspNetCore.Mvc;
using TheFinalProject.DAL;
using TheFinalProject.Entities;
using TheFinalProject.ViewModels.BlogAdmin;

namespace TheFinalProject.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    public class ServiceController : Controller
    {
        private readonly AppDbContext _appDbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ServiceController(AppDbContext appDbContext, IWebHostEnvironment webHostEnvironment)
        {
            _appDbContext = appDbContext;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            return View(_appDbContext.ServicePageMains.ToList());
        }
        public IActionResult Detail(int? id)
        {
            if (id == null) return NotFound();
            var existService = _appDbContext.ServicePageMains.FirstOrDefault(b => b.Id == id);
            if (existService == null) return NotFound();
            return View(existService);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Create(CreateServiceVM createServiceVM)

        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Photo", "Bos Qoyma");
                return View();
            }
            if (!createServiceVM.Img.ContentType.Contains("image/"))
            {
                ModelState.AddModelError("Photo", "only image");
                return View();
            }
            if (createServiceVM.Img.Length / 1024 > 1000)
            {
                ModelState.AddModelError("Photo", "Olchu boyukdur");
                return View();
            }
            ServicePageMain service = new();
            service.Title = createServiceVM.Title;
            service.Description = createServiceVM.Description;
            service.Img = createServiceVM.Img.SaveChanges("img/blog", _webHostEnvironment);
            _appDbContext.ServicePageMains.Add(service);
            _appDbContext.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int? id)
        {
            if (id == null) return NotFound();
            var deletedservice = _appDbContext.BlogPosts.FirstOrDefault(c => c.Id == id);
            if (deletedservice == null) return NotFound();

            Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<ServicePageMain> entityEntry = _appDbContext.ServicePageMains.Remove(deletedservice);
            _appDbContext.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Update(int? id)
        {
            if (id == null) return NotFound();
            var existservice = _appDbContext.ServicePageMains.FirstOrDefault(c => c.Id == id);
            if (existservice == null) return NotFound();
            var updateServiceVM = new UpdateServiceVM
            {
                Title = existservice.Title,
                Description = existservice.Description,

            };
            return View(updateServiceVM);

        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Update(UpdateServiceVM updateBlogVM)
        {
            if (!ModelState.IsValid) return View();
            var existblog = _appDbContext.BlogPosts.FirstOrDefault(c => c.Id == updateBlogVM.Id);
            if (updateBlogVM == null) return NotFound();

            existblog.Name = updateBlogVM.Name;
            existblog.Description = updateBlogVM.Description;

            if (updateBlogVM.Img != null)
            {

                if (!updateBlogVM.Img.ContentType.Contains("image/"))
                {
                    ModelState.AddModelError("Photo", "only image");
                    return View();
                }
                if (updateBlogVM.Img.Length / 1024 > 1000)
                {
                    ModelState.AddModelError("Photo", "Size is High");
                    return View();
                }

                existblog.Img = updateBlogVM.Img.SaveImage("img/blog", _webHostEnvironment);
            }

            _appDbContext.SaveChanges();
            return RedirectToAction("Index");

        }
    }
}
