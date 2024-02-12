using Microsoft.AspNetCore.Mvc;
using TheFinalProject.DAL;
using TheFinalProject.Entities;
using TheFinalProject.ViewModels.BlogAdmin;

namespace TheFinalProject.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    public class BlogController : Controller
    {
        private readonly AppDbContext _appDbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public BlogController(AppDbContext appDbContext, IWebHostEnvironment webHostEnvironment)
        {
            _appDbContext = appDbContext;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View(_appDbContext.BlogPosts.ToList());
        }
        public IActionResult Detail(int? id)
        {
            if (id == null) return NotFound();
            var existBlog = _appDbContext.BlogPosts.FirstOrDefault(b => b.Id == id);
            if (existBlog == null) return NotFound();
            return View(existBlog);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Create(CreateBlogVM createBlogVM)

        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Photo", "Bos Qoyma");
                return View();
            }
            if (!createBlogVM.Img.ContentType.Contains("image/"))
            {
                ModelState.AddModelError("Photo", "only image");
                return View();
            }
            if (createBlogVM.Img.Length / 1024 > 1000)
            {
                ModelState.AddModelError("Photo", "Olchu boyukdur");
                return View();
            }
            BlogPost blog = new();
            blog.Name = createBlogVM.Name;
            blog.Description = createBlogVM.Description;
            blog.Img = createBlogVM.Img.SaveChanges("img/blog", _webHostEnvironment);
            _appDbContext.BlogPosts.Add(blog);
            _appDbContext.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int? id)
        {
            if (id == null) return NotFound();
            var deletedblog = _appDbContext.BlogPosts.FirstOrDefault(c => c.Id == id);
            if (deletedblog == null) return NotFound();

            _appDbContext.BlogPosts.Remove(deletedblog);
            _appDbContext.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Update(int? id)
        {
            if (id == null) return NotFound();
            var existblog = _appDbContext.BlogPosts.FirstOrDefault(c => c.Id == id);
            if (existblog == null) return NotFound();
            var updateblogVM = new UpdateBlogVM
            {
                Name = existblog.Name,
                Description = existblog.Description,

            };
            return View(updateblogVM);

        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Update(UpdateBlogVM updateBlogVM)
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
