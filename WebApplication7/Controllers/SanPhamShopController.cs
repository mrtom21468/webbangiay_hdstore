using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Notyf;
using ClosedXML;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication7.Areas.Admin.Helpper;
using WebApplication7.Models;

namespace WebApplication7.Controllers
{
    public class SanPhamShopController : Controller
    {
        private readonly QLDBcontext _context;
        private readonly NotyfService _notyfService;

        public SanPhamShopController(QLDBcontext context,NotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        // GET: SanPhamShop
        public async Task<IActionResult> Index(int? thuonghieu, int? theloai, decimal? sort, int?min , decimal? max,string? search, int? pageNumber)
        {
            IQueryable<Product> query = _context.Products.Include(p => p.ProductDetails);
            if(search  != null){
                query = query.Where(p => p.ProductName.Contains(search));
                ViewBag.search = search;
            }
            if (thuonghieu != null)
            {
                query = query.Where(u => u.BrandId == thuonghieu);
            }

            if (theloai != null)
            {
                query = query.Where(u => u.CategoryId == theloai);
            }
            if (sort == 1) // Nếu sort có giá trị là 1, sắp xếp theo giá tăng dần
            {
                query = query.OrderBy(u => u.ProductDetails.First().SellingPrice);
            }
            else if (sort == 2) // Nếu sort có giá trị là 2, sắp xếp theo giá giảm dần
            {
                query = query.OrderByDescending(u => u.ProductDetails.First().SellingPrice);
            }
            if(min != null)
            {
                query = query.Where(u => u.ProductDetails.First().SellingPrice >= min && u.ProductDetails.First().SellingPrice <= max);
            }
            var productList = query.AsNoTracking();

            if (productList != null)
            {
                int pageSize = 5;
                return View(await PaginatedList<Product>.CreateAsync(productList, pageNumber ?? 1, pageSize));
            }
            _notyfService.Warning("Không có sản phẩm nào phù hợp",5);
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Search(string? search)
        {
            if(search == null)
            {
                return View("Index", await _context.Products
            .Include(p => p.ProductDetails)
            .ToListAsync());
            }
            var resul=await _context.Products
                .Include(p => p.ProductDetails)
                .Where(p=>p.ProductName.Contains(search))
                .ToListAsync();
            ViewBag.search = search;
            return View("Index",resul);
        }
        public async Task<IActionResult> SortProducts(int sortBy)
        {
            // Xử lý logic sắp xếp sản phẩm ở đây
            // Ví dụ:
            switch (sortBy)
            {
                case 1:
                    // Sắp xếp sản phẩm theo giá tăng dần
                    break;
                case 2:
                    // Sắp xếp sản phẩm theo giá giảm dần
                    break;
                default:
                    // Sắp xếp sản phẩm theo mặc định
                    break;
            }

            // Trả về kết quả nếu cần
            return View();
        }
    }
}
