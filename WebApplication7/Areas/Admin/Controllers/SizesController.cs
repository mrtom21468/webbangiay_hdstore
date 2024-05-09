using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication7.Models;
using WebApplication7.Areas.Admin.Helpper;
using AspNetCoreHero.ToastNotification.Notyf;

namespace WebApplication7.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SizesController : Controller
    {
        private readonly QLDBcontext _context;
        private readonly NotyfService _notyfService;

        public SizesController(QLDBcontext context, NotyfService notyfService)
        {
            _context = context;
            _notyfService= notyfService;
        }

        // GET: Admin/Sizes
        public async Task<IActionResult> Index()
        {
            return View(await _context.Sizes.ToListAsync());
        }
        // GET: Admin/Sizes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Sizes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SizeId,SizeName")] Size size)
        {
            if (ModelState.IsValid)
            {
                size.SizeSlug=Slug.GenerateSlug(size.SizeName);
                var checkSize= _context.Sizes.FirstOrDefault(m => m.SizeSlug == size.SizeSlug);
                if (checkSize == null)
                {
                    _context.Add(size);
                    await _context.SaveChangesAsync();
                    _notyfService.Success("Thêm thành công", 3);
                    return RedirectToAction(nameof(Index));
                }
                _notyfService.Error("Trùng size", 3);
                return View(size);

            }
            _notyfService.Error("Dữ liệu không hợp lệ", 3);
            return View(size);
        }

        // GET: Admin/Sizes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var size = await _context.Sizes.FindAsync(id);
            if (size == null)
            {
                return NotFound();
            }
            return View(size);
        }

        // POST: Admin/Sizes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SizeId,SizeName")] Size size)
        {
            if (id != size.SizeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    size.SizeSlug = Slug.GenerateSlug(size.SizeName);
                    var checkSize = _context.Sizes.FirstOrDefault(m => m.SizeSlug == size.SizeSlug && m.SizeId!=id);
                    if(checkSize != null)
                    {
                        _notyfService.Error("Trùng size", 3);
                        return View(size);
                    }
                    _context.Update(size);
                    await _context.SaveChangesAsync();
                    _notyfService.Success("Cập nhật thành công", 3);

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SizeExists(size.SizeId))
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
            return View(size);
        }

        // GET: Admin/Sizes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var size = await _context.Sizes
                .FirstOrDefaultAsync(m => m.SizeId == id);
            if (size == null)
            {
                return NotFound();
            }

            return View(size);
        }

        // POST: Admin/Sizes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var size = await _context.Sizes.FindAsync(id);
            if (size != null)
            {
                _context.Sizes.Remove(size);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SizeExists(int id)
        {
            return _context.Sizes.Any(e => e.SizeId == id);
        }
    }
}
