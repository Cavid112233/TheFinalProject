using Microsoft.AspNetCore.Mvc;
using TheFinalProject.DAL;
using TheFinalProject.Entities;
using TheFinalProject.ViewModels.ProductAdmin;

namespace TheFinalProject.Areas.AdminArea.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _appDbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(AppDbContext appDbContext, IWebHostEnvironment webHostEnvironment)
        {
            _appDbContext = appDbContext;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View(_appDbContext.Productss.ToList());
        }
        public IActionResult Detail(int? id)
        {
            if (id == null) return NotFound();
            var existProduct = _appDbContext.Productss.FirstOrDefault(b => b.Id == id);
            if (existProduct == null) return NotFound();
            return View(existProduct);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Create(CreateProductVM createProductVM)

        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Photo", "Bos Qoyma");
                return View();
            }
            if (!createProductVM.Img.Contains("image/"))
            {
                ModelState.AddModelError("Photo", "only image");
                return View();
            }
            if (createProductVM.Img.Length / 1024 > 1000)
            {
                ModelState.AddModelError("Photo", "Olchu boyukdur");
                return View();
            }
            Products product = new();
            product.Title = createProductVM.Title;
            product.OldPrice = createProductVM.OldPrice;
            product.NewPrice = createProductVM.NewPrice;
            product.Img = createProductVM.Img.SaveChanges("img/blog", _webHostEnvironment);
            _appDbContext.Productss.Add(product);
            _appDbContext.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int? id)
        {
            if (id == null) return NotFound();
            var deletedproduct = _appDbContext.Productss.FirstOrDefault(c => c.Id == id);
            if (deletedproduct == null) return NotFound();

            _appDbContext.Productss.Remove(deletedproduct);
            _appDbContext.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Update(int? id)
        {
            if (id == null) return NotFound();
            var existproduct = _appDbContext.Productss.FirstOrDefault(c => c.Id == id);
            if (existproduct == null) return NotFound();
            var updateproductVM = new UpdateProductVM
            {
                Title = existproduct.Title,
                OldPrice = existproduct.OldPrice,

            };
            return View(updateproductVM);

        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Update(UpdateProductVM updateproductVM)
        {
            if (!ModelState.IsValid) return View();
            var existproduct = _appDbContext.Productss.FirstOrDefault(c => c.Id == updateproductVM.Id);
            if (updateproductVM == null) return NotFound();

            existproduct.Title = updateproductVM.Title;
            existproduct.OldPrice = updateproductVM.OldPrice;
            existproduct.NewPrice = updateproductVM.NewPrice;

            if (updateproductVM.Img != null)
            {

                if (!updateproductVM.Img.Contains("image/"))
                {
                    ModelState.AddModelError("Photo", "only image");
                    return View();
                }
                if (updateproductVM.Img.Length / 1024 > 1000)
                {
                    ModelState.AddModelError("Photo", "Size is High");
                    return View();
                }

                existproduct.Img = updateproductVM.Img.SaveImage("img/blog", _webHostEnvironment);
            }

            _appDbContext.SaveChanges();
            return RedirectToAction("Index");

        }
    }
}
