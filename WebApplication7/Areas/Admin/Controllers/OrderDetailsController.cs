using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WebApplication7.Models;

namespace WebApplication7.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderDetailsController : Controller
    {
        private readonly QLDBcontext _context;

        public OrderDetailsController(QLDBcontext context)
        {
            _context = context;
        }

        // GET: Admin/OrderDetails
        public async Task<IActionResult> Index()
        {
            var qLDBcontext = _context.OrderDetails.Include(o => o.Order).Include(o => o.Productdetail);
            return View(await qLDBcontext.ToListAsync());
        }

        // GET: Admin/OrderDetails/Details/5
        public async Task<IActionResult> Details(int? id)
       {
            if (id == null)
            {
                return NotFound();
            }

            var orderDetail = await _context.OrderDetails
             .Include(o => o.Order)
            .Include(o => o.Productdetail)
                .ThenInclude(pd => pd.Color) // Bổ sung dữ liệu từ bảng Color
            .Include(o => o.Productdetail)
                .ThenInclude(pd => pd.Size) // Bổ sung dữ liệu từ bảng Size liên quan đến Productdetail
            .Include(o => o.Productdetail)
                .ThenInclude(pd => pd.Product) // Bổ sung dữ liệu từ bảng Product
            .Where(p => p.OrderId == id)
            .ToListAsync();


            if (orderDetail == null)
            {
                return NotFound();
            }

            return View(orderDetail);
        }
        public IActionResult ExportToExcel(int? id)
        {
            var orderDetails =  _context.OrderDetails
            .Include(o => o.Order)
            .Include(o => o.Productdetail)
                .ThenInclude(pd => pd.Color) // Bổ sung dữ liệu từ bảng Color
            .Include(o => o.Productdetail)  
                .ThenInclude(pd => pd.Size) // Bổ sung dữ liệu từ bảng Size liên quan đến Productdetail
            .Include(o => o.Productdetail)
                .ThenInclude(pd => pd.Product) // Bổ sung dữ liệu từ bảng Product
            .Where(p => p.OrderId == id)
            .ToList();
            if (orderDetails.Count == 0)
            {
                // Nếu không có dữ liệu, chuyển hướng đến trang khác hoặc hiển thị thông báo
                // Ví dụ: 
                return RedirectToAction("Index", "Home");
            }
            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Hóa đơn "+ orderDetails[0].OrderId);

            // Thêm dữ liệu vào các ô trong worksheet
            // Ví dụ:
            worksheet.Cell("A1").Value = "Tên sản phẩm";
            worksheet.Cell("B1").Value = "Màu sản phẩm";
            worksheet.Cell("C1").Value = "Size sản phẩm";
            worksheet.Cell("D1").Value = "Số lượng";

            // Lặp qua dữ liệu hóa đơn và thêm vào các dòng tiếp theo trong worksheet
            int row = 2;
            foreach (var orderDetail in orderDetails)
            {
                worksheet.Cell("A" + row).Value = orderDetail.Productdetail.Product.ProductName;
                worksheet.Cell("B" + row).Value = orderDetail.Productdetail.Color.ColorName;
                worksheet.Cell("C" + row).Value = orderDetail.Productdetail.Size.SizeName;
                worksheet.Cell("D" + row).Value = orderDetail.Quantity;
                row++;
            }
            var hoadonname = "Hóa đơn" + orderDetails[0].OrderId +".xlsx";
            // Lưu workbook vào một MemoryStream
            using (var stream = new MemoryStream())
            {
                workbook.SaveAs(stream);
                var content = stream.ToArray();

                // Cập nhật trạng thái của đơn hàng thành "đã đóng gói" trong cơ sở dữ liệu
                var order = _context.Orders.FirstOrDefault(o => o.OrderId == id);
                if (order != null)
                {
                    order.Status = "Đã đóng gói";
                    _context.SaveChanges();
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", hoadonname);
                }

                // Chuyển hướng người dùng đến action khác sau khi xuất hóa đơn
                return RedirectToAction("Index", "Orders");
            }
        }
        // GET: Admin/OrderDetails/Create
        public IActionResult Create()
        {
            ViewData["OrderId"] = new SelectList(_context.Orders, "OrderId", "OrderId");
            ViewData["ProductdetailId"] = new SelectList(_context.ProductDetails, "ProductdetailId", "ProductdetailId");
            return View();
        }

        // POST: Admin/OrderDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderDetailId,OrderId,ProductdetailId,Quantity")] OrderDetail orderDetail)
        {
            if (ModelState.IsValid)
            {
                _context.Add(orderDetail);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["OrderId"] = new SelectList(_context.Orders, "OrderId", "OrderId", orderDetail.OrderId);
            ViewData["ProductdetailId"] = new SelectList(_context.ProductDetails, "ProductdetailId", "ProductdetailId", orderDetail.ProductdetailId);
            return View(orderDetail);
        }

        // GET: Admin/OrderDetails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderDetail = await _context.OrderDetails.FindAsync(id);
            if (orderDetail == null)
            {
                return NotFound();
            }
            ViewData["OrderId"] = new SelectList(_context.Orders, "OrderId", "OrderId", orderDetail.OrderId);
            ViewData["ProductdetailId"] = new SelectList(_context.ProductDetails, "ProductdetailId", "ProductdetailId", orderDetail.ProductdetailId);
            return View(orderDetail);
        }

        // POST: Admin/OrderDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderDetailId,OrderId,ProductdetailId,Quantity")] OrderDetail orderDetail)
        {
            if (id != orderDetail.OrderDetailId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(orderDetail);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderDetailExists(orderDetail.OrderDetailId))
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
            ViewData["OrderId"] = new SelectList(_context.Orders, "OrderId", "OrderId", orderDetail.OrderId);
            ViewData["ProductdetailId"] = new SelectList(_context.ProductDetails, "ProductdetailId", "ProductdetailId", orderDetail.ProductdetailId);
            return View(orderDetail);
        }

        // GET: Admin/OrderDetails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderDetail = await _context.OrderDetails
                .Include(o => o.Order)
                .Include(o => o.Productdetail)
                .FirstOrDefaultAsync(m => m.OrderDetailId == id);
            if (orderDetail == null)
            {
                return NotFound();
            }

            return View(orderDetail);
        }

        // POST: Admin/OrderDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var orderDetail = await _context.OrderDetails.FindAsync(id);
            if (orderDetail != null)
            {
                _context.OrderDetails.Remove(orderDetail);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderDetailExists(int id)
        {
            return _context.OrderDetails.Any(e => e.OrderDetailId == id);
        }
    }
}
