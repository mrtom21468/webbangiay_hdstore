using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication7.Areas.Admin.Helpper;
using WebApplication7.Models;
namespace WebApplication7.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrdersController : Controller
    {
        private readonly QLDBcontext _context;
        private readonly UserManager<UserIdentitycs> _userManager;

        public OrdersController(QLDBcontext context, UserManager<UserIdentitycs> userManager)
        {
            _context = context; 
            _userManager = userManager;
        }

        // GET: Admin/Orders
        public async Task<IActionResult> Index(int? pageNumber, string? orderStatus, string? paymentStatus, int? paymentType, DateTime? startDate, DateTime? endDate)
        {
            var qLDBcontext = _context.Orders
                                    .Include(o => o.Account)
                                    .OrderByDescending(o => o.CreatedAt)
                                    .AsNoTracking();
            if (orderStatus != null)
            {
                qLDBcontext = qLDBcontext.Where(o => o.Status == orderStatus);
            }
            if (paymentStatus !=null)
            {
                qLDBcontext = qLDBcontext.Where(o => o.PaymentStatus == paymentStatus);
            }
            if (paymentType != null)
            {
                var typepay = paymentType == 0 ? "Tiền mặt" : (paymentType == 1 ? "Ví Momo" : "Thẻ ngân hàng");
                qLDBcontext = qLDBcontext.Where(o => o.PaymentType == typepay);

            }
            if(startDate!=null && endDate != null)
            {
                qLDBcontext = qLDBcontext.Where(o => o.CreatedAt>= startDate && o.CreatedAt<= endDate);

            }
            int pageSize = 10;
            return View(await PaginatedList<Order>.CreateAsync(qLDBcontext, pageNumber ?? 1, pageSize));
        }
        // GET: Admin/Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Account)
                .FirstOrDefaultAsync(m => m.OrderId == id);
/*
            ViewBag.Name = _userManager.Users.Where(u => u.Id == order.Account.AccountId);*/
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }
        // GET: Admin/Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["AccountId"] = new SelectList(_context.Accounts, "AccountId", "AccountId", order.AccountId);
            // Define your status options
            var statusOptions = new List<SelectListItem>
            {
            new SelectListItem { Value = "1", Text = "Chờ xử lý" },
            new SelectListItem { Value = "2", Text = "Đã xác nhận" },
            new SelectListItem { Value = "3", Text = "Đã đóng gói" },
            new SelectListItem { Value = "4", Text = "Đang vận chuyển" },
            new SelectListItem { Value = "5", Text = "Hoàn thành" },
            new SelectListItem { Value = "6", Text = "Đã hủy" }
            };

            // Create a SelectList from the status options
            ViewBag.Statuses = new SelectList(statusOptions, "Value", "Text");

            // In your view, use the SelectList to create a dropdown list

            return View(order);
        }

        // POST: Admin/Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderId,AccountId,Address,PhoneNumber,PaymentType,PaymentStatus,Status,TotalAmount,CreatedAt,OrderIdMoMo,ReqrIdMoMo ")] Order order)
        {
            if (id != order.OrderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if(order.ReqrIdMoMo==null || order.OrderIdMoMo == null)
                    {
                        order.OrderIdMoMo = "-";
                        order.ReqrIdMoMo = "-";
                    }
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.OrderId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AccountId"] = new SelectList(_context.Accounts, "AccountId", "AccountId", order.AccountId);
            return View(order);
        }
        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.OrderId == id);
        }
    }
}
