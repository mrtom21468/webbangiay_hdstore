using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication7.Models;

namespace WebApplication7.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductDetailsController : Controller
    {
        private readonly QLDBcontext _context;

        public ProductDetailsController(QLDBcontext context)
        {
            _context = context;
        }

        // GET: Admin/ProductDetails
        public async Task<IActionResult> Index()
        {
            var qLDBcontext = _context.ProductDetails.Include(p => p.Color).Include(p => p.Product).Include(p => p.Size);
            return View(await qLDBcontext.ToListAsync());
        }

        // GET: Admin/ProductDetails/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productDetail = await _context.ProductDetails
                .Include(p => p.Color)
                .Include(p => p.Product)
                .Include(p => p.Size)
                .FirstOrDefaultAsync(m => m.ProductdetailId == id);
            if (productDetail == null)
            {
                return NotFound();
            }

            return View(productDetail);
        }

        // GET: Admin/ProductDetails/Create
        public IActionResult Create(int? id)
        {
            ViewData["ColorId"] = new SelectList(_context.Colors, "ColorId", "ColorName");
            if (id != null)
            {
                
                ViewData["ProductId"] = new SelectList(_context.Products.Where(p=>p.ProductId == id), "ProductId", "ProductName");
            }
            else
            {
                ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductName");

            }
            ViewData["SizeId"] = new SelectList(_context.Sizes, "SizeId", "SizeName");
            return View();
        }

        // POST: Admin/ProductDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductdetailId,ProductId,SizeId,ColorId,Quantity,SellingPrice")] ProductDetail productDetail)
        {
            if (ModelState.IsValid)
            {
                _context.Add(productDetail);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ColorId"] = new SelectList(_context.Colors, "ColorId", "ColorName", productDetail.ColorId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductName", productDetail.ProductId);
            ViewData["SizeId"] = new SelectList(_context.Sizes, "SizeId", "SizeName", productDetail.SizeId);
            return View(productDetail);
        }

        // GET: Admin/ProductDetails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productDetail = await _context.ProductDetails.FindAsync(id);
            if (productDetail == null)
            {
                return NotFound();
            }
            ViewData["ColorId"] = new SelectList(_context.Colors, "ColorId", "ColorName", productDetail.ColorId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductName", productDetail.ProductId);
            ViewData["SizeId"] = new SelectList(_context.Sizes, "SizeId", "SizeName", productDetail.SizeId);
            return View(productDetail);
        }

        // POST: Admin/ProductDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductdetailId,ProductId,SizeId,ColorId,Quantity,SellingPrice")] ProductDetail productDetail)
        {
            if (id != productDetail.ProductdetailId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productDetail);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductDetailExists(productDetail.ProductdetailId))
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
            ViewData["ColorId"] = new SelectList(_context.Colors, "ColorId", "ColorName", productDetail.ColorId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductName", productDetail.ProductId);
            ViewData["SizeId"] = new SelectList(_context.Sizes, "SizeId", "SizeName", productDetail.SizeId);
            return View(productDetail);
        }

        // GET: Admin/ProductDetails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productDetail = await _context.ProductDetails
                .Include(p => p.Color)
                .Include(p => p.Product)
                .Include(p => p.Size)
                .FirstOrDefaultAsync(m => m.ProductdetailId == id);
            if (productDetail == null)
            {
                return NotFound();
            }

            return View(productDetail);
        }

        // POST: Admin/ProductDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productDetail = await _context.ProductDetails.FindAsync(id);
            if (productDetail != null)
            {
                _context.ProductDetails.Remove(productDetail);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductDetailExists(int id)
        {
            return _context.ProductDetails.Any(e => e.ProductdetailId == id);
        }
    }
}
