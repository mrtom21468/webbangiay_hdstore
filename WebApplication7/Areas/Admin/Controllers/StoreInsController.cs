using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication7.Models;

namespace WebApplication7.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class StoreInsController : Controller
    {
        private readonly QLDBcontext _context;
        private readonly UserManager<UserIdentitycs> _userManager;

        public StoreInsController(QLDBcontext context, UserManager<UserIdentitycs> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Admin/StoreIns
        public async Task<IActionResult> Index()
        {
            var qLDBcontext = _context.StoreIns.Include(s => s.Account)
                .Include(s => s.ProductDetail)
                .ThenInclude(p=>p.Product)
                .Include(s => s.ProductDetail)
                .ThenInclude(p=>p.Size)
                .Include(s => s.ProductDetail)
                .ThenInclude(p => p.Color)
                .Include(s => s.Supplier)
                .OrderByDescending(s=>s.StoreInId);
                
            return View(await qLDBcontext.ToListAsync());
        }

        // GET: Admin/StoreIns/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var storeIn = await _context.StoreIns
                .Include(s => s.Account)
                .Include(s => s.ProductDetail)
                .Include(s => s.Supplier)
                .FirstOrDefaultAsync(m => m.StoreInId == id);
            if (storeIn == null)
            {
                return NotFound();
            }

            return View(storeIn);
        }

        // GET: Admin/StoreIns/Create
        public IActionResult Create()
        {
            var products = _context.ProductDetails
                                .Include(p => p.Product)
                                .Include(c => c.Color)
                                .Include(s => s.Size)
                                .OrderBy(s => s.Product.ProductName)
                                .ThenBy(s => s.Color.ColorName)
                                .ThenBy(s => s.Size.SizeName)
                            .Select(s => new
            {
                ProductDetailId = s.ProductdetailId,
                FullName = $"{s.Product.ProductName} - {s.Color.ColorName} - {s.Size.SizeName}" // Kết hợp tên và số điện thoại của nhà cung cấp trong cùng một chuỗi
            })
            .ToList();
            ViewData["ProductdetailId"] = new SelectList(products, "ProductDetailId", "FullName");
            // Lấy danh sách nhà cung cấp từ cơ sở dữ liệu
            var suppliers = _context.Suppliers.Select(s => new
            {
                SupplierId = s.SupplierId,
                FullName = $"{s.SupplierName} - {s.PhoneNumber}" // Kết hợp tên và số điện thoại của nhà cung cấp trong cùng một chuỗi
            }).ToList();
            ViewData["SupplierId"] = new SelectList(suppliers, "SupplierId", "FullName");
            return View();
        }

        // POST: Admin/StoreIns/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StoreInId,ProductdetailId,SupplierId,ImprortPrice,Quantity")] StoreIn storeIn)
        {
            if (ModelState.IsValid)
            {
                var userid = _userManager.GetUserId(User);
                var user = _context.Accounts.Where(u => u.UserId == userid).Select(u => u.AccountId).FirstOrDefaultAsync();
                if(user.Result!= null)
                {
                    storeIn.AccountId = user.Result;
                }
                var updateproductDetails = _context.ProductDetails.FirstOrDefault(p => p.ProductdetailId == storeIn.ProductdetailId);
                if (updateproductDetails != null)
                {
                    updateproductDetails.PurchasePrice = (updateproductDetails.Quantity * updateproductDetails.Quantity + storeIn.ImprortPrice * storeIn.Quantity) / (updateproductDetails.Quantity + storeIn.Quantity);
                    updateproductDetails.Quantity += storeIn.Quantity;
                    
                    // Cập nhật thông tin của bản ghi trong cơ sở dữ liệu
                    _context.Update(updateproductDetails);
                }
                _context.Add(storeIn);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AccountId"] = new SelectList(_context.Accounts, "AccountId", "AccountId", storeIn.AccountId);
            var products = _context.ProductDetails
                            .Include(p => p.Product)
                            .Include(c => c.Color)
                            .Include(s => s.Size)
                .Select(s => new
                {
                    ProductDetailId = s.ProductdetailId,
                    FullName = $"{s.Product.ProductName} - {s.Color.ColorName} - {s.Size.SizeName}" // Kết hợp tên và số điện thoại của nhà cung cấp trong cùng một chuỗi
                }).ToList();
            ViewData["ProductdetailId"] = new SelectList(products, "ProductDetailId", "FullName", storeIn.ProductdetailId);            // Lấy danh sách nhà cung cấp từ cơ sở dữ liệu
            var suppliers = _context.Suppliers.Select(s => new
            {
                SupplierId = s.SupplierId,
                FullName = $"{s.SupplierName} - {s.PhoneNumber}" // Kết hợp tên và số điện thoại của nhà cung cấp trong cùng một chuỗi
            }).ToList();
            ViewData["SupplierId"] = new SelectList(suppliers, "SupplierId", "FullName", storeIn.SupplierId);
            return View(storeIn);
        }

        // GET: Admin/StoreIns/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var storeIn = await _context.StoreIns.FindAsync(id);
            if (storeIn == null)
            {
                return NotFound();
            }
            ViewData["AccountId"] = new SelectList(_context.Accounts, "AccountId", "AccountId", storeIn.AccountId);
            var products = _context.ProductDetails
                            .Include(p => p.Product)
                            .Include(c => c.Color)
                            .Include(s => s.Size)
                .Select(s => new
                {
                    ProductDetailId = s.ProductdetailId,
                    FullName = $"{s.Product.ProductName} - {s.Color.ColorName} - {s.Size.SizeName}" // Kết hợp tên và số điện thoại của nhà cung cấp trong cùng một chuỗi
                }).ToList();
            ViewData["ProductdetailId"] = new SelectList(products, "ProductDetailId", "FullName", storeIn.ProductdetailId);            // Lấy danh sách nhà cung cấp từ cơ sở dữ liệu
            var suppliers = _context.Suppliers.Select(s => new
            {
                SupplierId = s.SupplierId,
                FullName = $"{s.SupplierName} - {s.PhoneNumber}" // Kết hợp tên và số điện thoại của nhà cung cấp trong cùng một chuỗi
            }).ToList();
            ViewData["SupplierId"] = new SelectList(suppliers, "SupplierId", "FullName", storeIn.SupplierId);
            return View(storeIn);
        }

        // POST: Admin/StoreIns/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StoreInId,ProductdetailId,AccountId,SupplierId,CreatedAt,ImprortPrice,Quantity")] StoreIn storeIn)
        {
            if (id != storeIn.StoreInId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(storeIn);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StoreInExists(storeIn.StoreInId))
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
            ViewData["AccountId"] = new SelectList(_context.Accounts, "AccountId", "AccountId", storeIn.AccountId);
            var products = _context.ProductDetails
                            .Include(p => p.Product)
                            .Include(c => c.Color)
                            .Include(s => s.Size)
                .Select(s => new
                {
                    ProductDetailId = s.ProductdetailId,
                    FullName = $"{s.Product.ProductName} - {s.Color.ColorName} - {s.Size.SizeName}" // Kết hợp tên và số điện thoại của nhà cung cấp trong cùng một chuỗi
                }).ToList();
            ViewData["ProductdetailId"] = new SelectList(products, "ProductDetailId", "FullName", storeIn.ProductdetailId);            // Lấy danh sách nhà cung cấp từ cơ sở dữ liệu
            var suppliers = _context.Suppliers.Select(s => new
            {
                SupplierId = s.SupplierId,
                FullName = $"{s.SupplierName} - {s.PhoneNumber}" // Kết hợp tên và số điện thoại của nhà cung cấp trong cùng một chuỗi
            }).ToList();
            ViewData["SupplierId"] = new SelectList(suppliers, "SupplierId", "FullName", storeIn.SupplierId);
            return View(storeIn);
        }

        // GET: Admin/StoreIns/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var storeIn = await _context.StoreIns
                .Include(s => s.Account)
                .Include(s => s.ProductDetail)
                .Include(s => s.Supplier)
                .FirstOrDefaultAsync(m => m.StoreInId == id);
            if (storeIn == null)
            {
                return NotFound();
            }

            return View(storeIn);
        }

        // POST: Admin/StoreIns/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var storeIn = await _context.StoreIns.FindAsync(id);
            if (storeIn != null)
            {
                _context.StoreIns.Remove(storeIn);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StoreInExists(int id)
        {
            return _context.StoreIns.Any(e => e.StoreInId == id);
        }
    }
}
