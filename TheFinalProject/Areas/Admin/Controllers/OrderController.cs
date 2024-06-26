using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace TheFinalProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class OrderController : Controller
    {
        private readonly AppDbContext _context;

        public OrderController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(int pageIndex = 1)
        {
            IQueryable<Order> orders = _context.Orders.Include(o => o.OrderItems);

            return View(PagenatedList<Order>.Create(orders, pageIndex, 5));
        }
        [HttpGet]

        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return BadRequest();

            Order order = await _context.Orders
                .Include(o => o.OrderItems.Where(oi => oi.IsDeleted == false))
                .ThenInclude(oi => oi.Product).FirstOrDefaultAsync(o => o.IsDeleted == false && o.Id == id);
            if (order == null) return NotFound();


            return View(order);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> changeStatus(Order order)
        {
            if (order.Id <= 0)
            {
                return BadRequest();
            }
            Order dbOrder = await _context.Orders
                .Include(o => o.OrderItems.Where(oi => oi.IsDeleted == false))
                .ThenInclude(oi => oi.Product)
               .FirstOrDefaultAsync(o => o.IsDeleted == false && o.Id == order.Id);
            if (dbOrder == null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid) return View("Detail", dbOrder);



            dbOrder.Status = order.Status;
            dbOrder.Comment = order.Comment;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }
    }
}
