using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Notyf;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication7.Areas.Admin.Helpper;
using WebApplication7.Models;

namespace WebApplication7.Controllers
{
    public class OrdersController : Controller
    {
        private readonly QLDBcontext _context;
        private readonly NotyfService _notyfService;
        private readonly UserManager<UserIdentitycs> _userManager;

        public OrdersController(QLDBcontext context, NotyfService notyfService, UserManager<UserIdentitycs> userManager)
        {
            _context = context;
            _notyfService = notyfService;
            _userManager = userManager;
        }


        // GET: Admin/Orders
        public async Task<IActionResult> Index(int? pageNumber, string? orderStatus, string? paymentStatus, int? paymentType, DateTime? startDate, DateTime? endDate)
        {
            var userid = _userManager.GetUserId(User);
            var user = _context.Accounts.Where(u => u.UserId == userid).Select(u => u.AccountId).FirstOrDefaultAsync();
            if (user != null)
            {
                var qLDBcontext = _context.Orders
                                        .Include(o => o.Account)
                                        .Where(o=>o.AccountId==user.Result)
                                        .OrderByDescending(o => o.CreatedAt)
                                        .AsNoTracking();
                if (orderStatus != null)
                {
                    qLDBcontext = qLDBcontext.Where(o => o.Status == orderStatus);
                }
                if (paymentStatus != null)
                {
                    qLDBcontext = qLDBcontext.Where(o => o.PaymentStatus == paymentStatus);
                }
                if (paymentType != null)
                {
                    var typepay = paymentType == 0 ? "Tiền mặt" : (paymentType == 1 ? "Ví Momo" : (paymentType == 2 ? "Thẻ ngân hàng" : "Thẻ thanh toán quốc tế"));
                    qLDBcontext = qLDBcontext.Where(o => o.PaymentType == typepay);

                }
                if (startDate != null && endDate != null)
                {
                    qLDBcontext = qLDBcontext.Where(o => o.CreatedAt >= startDate && o.CreatedAt <= endDate);

                }
                int pageSize = 10;
                return View(await PaginatedList<Order>.CreateAsync(qLDBcontext, pageNumber ?? 1, pageSize));
            }
            return RedirectToAction("Index", "Home");
        }
        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var userid = _userManager.GetUserId(User);
            var user = _context.Accounts.Where(u => u.UserId == userid).Select(u => u.AccountId).FirstOrDefaultAsync();
            var orderDetail = await _context.OrderDetails
             .Include(o => o.Order)
            .Include(o => o.Productdetail)
                .ThenInclude(pd => pd.Color) // Bổ sung dữ liệu từ bảng Color
            .Include(o => o.Productdetail)
                .ThenInclude(pd => pd.Size) // Bổ sung dữ liệu từ bảng Size liên quan đến Productdetail
            .Include(o => o.Productdetail)
                .ThenInclude(pd => pd.Product) // Bổ sung dữ liệu từ bảng Product
            .Where(p => p.OrderId == id && p.Order.AccountId== user.Result)
            .ToListAsync();


            if (orderDetail == null)
            {
                return NotFound();
            }

            return View(orderDetail);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Huy(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var userid = _userManager.GetUserId(User);
            var user = _context.Accounts.Where(u => u.UserId == userid).Select(u => u.AccountId).FirstOrDefaultAsync();
            var order = await _context.Orders
                .Include(o=>o.OrderDetails)
                .ThenInclude(p=>p.Productdetail)
                .FirstOrDefaultAsync(m => m.OrderId == id && m.AccountId == user.Result);
            if (order == null)
            {
                return NotFound();
            }
            if(order.Status=="1" && order.PaymentStatus=="3") {
                order.Status = "6";
                _context.SaveChanges();
                //cập nhật lại số lượng sản phẩm
                foreach(var i in order.OrderDetails)
                {
                    //lấy sản phẩm cần cập nhật lại số lượng:
                    var sp=_context.ProductDetails.Where(p=>p.ProductdetailId==i.Productdetail.ProductdetailId).FirstOrDefault();
                    sp.Quantity += i.Quantity;
                    _context.SaveChanges();
                }
                _notyfService.Success("Hủy đơn hàng thành công", 3);
            }
            else
            {
                _notyfService.Error("Hủy đơn hàng thất bại liên hệ chúng tôi để xử lý", 3);
            }
            return RedirectToAction("Index");
        }
    }
}
