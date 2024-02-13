using Microsoft.AspNetCore.Mvc;
using TheFinalProject.DAL;
using TheFinalProject.Entities;
using TheFinalProject.Extension;
using TheFinalProject.ViewModels.BlogAdmin;
using TheFinalProject.ViewModels.ExperiencedStaffAdmin;

namespace TheFinalProject.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    public class ExperiencedStaffController : Controller
    {
        private readonly AppDbContext _appDbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ExperiencedStaffController(AppDbContext appDbContext, IWebHostEnvironment webHostEnvironment)
        {
            _appDbContext = appDbContext;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            return View(_appDbContext.ExperiencedStaffs.ToList());
        }
        public IActionResult Detail(int? id)
        {
            if (id == null) return NotFound();
            var existExperienced = _appDbContext.ExperiencedStaffs.FirstOrDefault(b => b.Id == id);
            if (existExperienced == null) return NotFound();
            return View(existExperienced);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Create(CreateExperiencedStaffVM createExperiencedVM)

        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Photo", "Bos Qoyma");
                return View();
            }
            if (!createExperiencedVM.Img.Contains("image/"))
            {
                ModelState.AddModelError("Photo", "only image");
                return View();
            }
            if (createExperiencedVM.Img.Length / 1024 > 1000)
            {
                ModelState.AddModelError("Photo", "Olchu boyukdur");
                return View();
            }
            ExperiencedStaff ExperiencedStaffss = new();
            ExperiencedStaffss.Name = createExperiencedVM.Name;
            ExperiencedStaffss.Img = createExperiencedVM.Img.SaveChanges("img/blog", _webHostEnvironment);
            _appDbContext.ExperiencedStaffs.Add(ExperiencedStaffss);
            _appDbContext.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int? id)
        {
            if (id == null) return NotFound();
            var deletedexperienced = _appDbContext.ExperiencedStaffs.FirstOrDefault(c => c.Id == id);
            if (deletedexperienced == null) return NotFound();

            _appDbContext.ExperiencedStaffs.Remove(deletedexperienced);
            _appDbContext.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Update(int? id)
        {
            if (id == null) return NotFound();
            var existExperienced = _appDbContext.ExperiencedStaffs.FirstOrDefault(c => c.Id == id);
            if (existExperienced == null) return NotFound();
            var updateexperiencedVM = new UpdateExperiencedStaffVM
            {
                Name = existExperienced.Name,

            };
            return View(updateexperiencedVM);

        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Update(UpdateExperiencedStaffVM updateexperiencedVM)
        {
            if (!ModelState.IsValid) return View();
            var existExperienced = _appDbContext.ExperiencedStaffs.FirstOrDefault(c => c.Id == updateexperiencedVM.Id);
            if (updateexperiencedVM == null) return NotFound();

            existExperienced.Name = updateexperiencedVM.Name;

            if (updateexperiencedVM.Img != null)
            {

                if (!updateexperiencedVM.Img.Contains("image/"))
                {
                    ModelState.AddModelError("Photo", "only image");
                    return View();
                }
                if (updateexperiencedVM.Img.Length / 1024 > 1000)
                {
                    ModelState.AddModelError("Photo", "Size is High");
                    return View();
                }

            }

            _appDbContext.SaveChanges();
            return RedirectToAction("Index");

        }
    }
}
