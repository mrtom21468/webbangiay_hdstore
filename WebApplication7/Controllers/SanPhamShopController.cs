using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication7.Models;

namespace WebApplication7.Controllers
{
    public class SanPhamShopController : Controller
    {
        private readonly QLDBcontext _context;

        public SanPhamShopController(QLDBcontext context)
        {
            _context = context;
        }

        // GET: SanPhamShop
        public async Task<IActionResult> Index(int? thuonghieu, int?theloai)
        {
            if(thuonghieu == null && theloai==null)
            {
                return View(await _context.Products
            .Include(p => p.ProductDetails)
            .ToListAsync());
            }
            if (theloai == null && thuonghieu != null)
            {
                var list = await _context.Products
            .Where(u => u.BrandId == thuonghieu)
            .Include(p => p.ProductDetails)
            .ToListAsync();

                if (list.Count > 0)
                {
                    return View(list);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            if(thuonghieu == null && theloai != null)
            {
                var list = await _context.Products
           .Where(u => u.CategoryId == theloai)
           .Include(p => p.ProductDetails)
           .ToListAsync();

                if (list.Count > 0)
                {
                    return View(list);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            var xlist = await _context.Products
           .Where(u => u.CategoryId == theloai && u.BrandId== thuonghieu)
           .Include(p => p.ProductDetails)
           .ToListAsync();
            if (xlist.Count > 0) {
                return View(xlist);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
