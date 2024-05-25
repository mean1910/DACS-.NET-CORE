using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
using X.PagedList;
using static WebApplication1.Areas.Admin.Controllers.ADProductsController;

namespace WebApplication1.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class ADProductsController : Controller
    {
        private readonly ThaoDuocMarketContext _context;
        private object status;
        
        

        public ADProductsController(ThaoDuocMarketContext context)
        {
            _context = context;
        }

        // GET: Admin/ADProducts

        public ActionResult Index(int page = 1, int CatID = 0, string search = "")
        {
            var pageNumber = page;
            var pageSize = 10;

            IQueryable<Product> productsQuery = _context.Products
                                            .AsNoTracking()
                                            .Include(x => x.Cat)
                                            .OrderByDescending(x => x.ProductId);

            if (CatID != 0)
            {
                productsQuery = productsQuery.Where(x => x.CatId == CatID);
            }

            if (!string.IsNullOrEmpty(search))
            {
                productsQuery = productsQuery.Where(p => p.ProductName.Contains(search) || p.Description.Contains(search));
            }

            List<Product> lsProduct = productsQuery.ToList();

            PagedList<Product> models = new PagedList<Product>(lsProduct.AsQueryable(), pageNumber, pageSize);

            ViewBag.CurrentCateID = CatID;
            ViewBag.CurrentPage = pageNumber;
            ViewData["CatId"] = new SelectList(_context.Categories, "CatId", "CatName", CatID);

            return View(models);
        }


        // GET: Admin/ADProducts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Cat)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["CategoryName"] = product.Cat?.CatName;
            return View(product);
        }


        


        // GET: Admin/ADProducts/Create
        public IActionResult Create()
        {
            ViewData["CatId"] = new SelectList(_context.Categories, "CatId", "CatName");
            return View();
        }

        // POST: Admin/ADProducts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product, IFormFile thumbUrl)
        {
            if (ModelState.IsValid)
            {
                if (thumbUrl != null && thumbUrl.Length > 0)
                {
                    // Lưu hình ảnh đại diện
                    product.Thumb = await SaveImage(thumbUrl);
                }

                product.DateModified = DateTime.Now;
                product.DateCreate = DateTime.Now;

                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CatId"] = new SelectList(_context.Categories, "CatId", "CatName", product.CatId);
            return View(product);
        }

        private async Task<string> SaveImage(IFormFile thumb)
        {
            var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/thumb/product");

            // Đảm bảo thư mục tồn tại
            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }

            var fileName = Path.GetFileName(thumb.FileName);
            var filePath = Path.Combine(savePath, fileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await thumb.CopyToAsync(fileStream);
            }
            return "/thumb/product/" + fileName;
        }


        // GET: Admin/ADProducts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["CatId"] = new SelectList(_context.Categories, "CatId", "CatName", product.CatId);
            return View(product);
        }

        // POST: Admin/ADProducts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,ProductName,ShortDesc,Description,CatId,Price,Discount,Thumb,DateCreate,DateModified,BestSeller,HomeFlag,Active,Tags,UnitsInStock")] Product product, IFormFile thumbUrl)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (thumbUrl != null)
                    {
                        // Lưu hình ảnh đại diện mới
                        product.Thumb = await SaveImage(thumbUrl);
                    }
                    product.DateModified =DateTime.Now;
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                   
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
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
            ViewData["CatId"] = new SelectList(_context.Categories, "CatId", "CatName", product.CatId);
            return View(product);
        }

        // GET: Admin/ADProducts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Cat)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Admin/ADProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }

        public IActionResult Filtter(int CatID = 0)
        {
            var url = $"/Admin/AdminProducts?CatID={CatID}";
            if (CatID == 0)
            {
                url = "/Admin/AdminProducts";
            }
            return Json(new { status = "success", redirectUrl = url });
        }
    }
}
