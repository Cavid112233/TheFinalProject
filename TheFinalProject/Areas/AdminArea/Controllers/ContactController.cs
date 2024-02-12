using Microsoft.AspNetCore.Mvc;
using TheFinalProject.DAL;
using TheFinalProject.Entities;
using TheFinalProject.ViewModels.ContactAdmin;

namespace TheFinalProject.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    public class ContactController : Controller
    {
        private readonly AppDbContext _appDbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ContactController(AppDbContext appDbContext, IWebHostEnvironment webHostEnvironment)
        {
            _appDbContext = appDbContext;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            return View(_appDbContext.GetInTouches.ToList());
        }

        public IActionResult Detail(int? id)
        {
            if (id == null) return NotFound();
            var existContact = _appDbContext.GetInTouches.FirstOrDefault(b => b.Id == id);
            if (existContact == null) return NotFound();
            return View(existContact);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]

        public IActionResult Create(CreateContactVM createContactVM)

        {
            GetInTouch contact = new();
            contact.Name = createContactVM.Name;
            contact.PhoneNumber = createContactVM.PhoneNumber;
            contact.Email = createContactVM.Email;
            _appDbContext.GetInTouches.Add(contact);
            _appDbContext.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int? id)
        {
            if (id == null) return NotFound();
            var deletedcontact = _appDbContext.GetInTouches.FirstOrDefault(c => c.Id == id);
            if (deletedcontact == null) return NotFound();

            _appDbContext.GetInTouches.Remove(deletedcontact);
            _appDbContext.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Update(int? id)
        {
            if (id == null) return NotFound();
            var existContact = _appDbContext.GetInTouches.FirstOrDefault(c => c.Id == id);
            if (existContact == null) return NotFound();
            var updateContactVM = new UpdateContactVM
            {
                Name = existContact.Name,
                Email = existContact.Email,

            };
            return View(updateContactVM);

        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Update(UpdateContactVM updateContactVM)
        {
            if (!ModelState.IsValid) return View();
            var existContact = _appDbContext.GetInTouches.FirstOrDefault(c => c.Id == updateContactVM.Id);
            if (existContact == null) return NotFound();

            existContact.Name = updateContactVM.Name;
            existContact.Email = updateContactVM.Email;

           

            _appDbContext.SaveChanges();
            return RedirectToAction("Index");

        }
    }
}
