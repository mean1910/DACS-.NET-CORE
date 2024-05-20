using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ADPostsController : Controller
    {
        private readonly ThaoDuocMarketContext _context;

        public ADPostsController(ThaoDuocMarketContext context)
        {
            _context = context;
        }

        // GET: Admin/ADPosts
        public async Task<IActionResult> Index()
        {
            return View(await _context.Posts.ToListAsync());
        }

        // GET: Admin/ADPosts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .FirstOrDefaultAsync(m => m.PostId == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // GET: Admin/ADPosts/Create
        public IActionResult Create()
        {
            return View();
        }



        private async Task<string> SaveImage(IFormFile thumb)
        {
            var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/thumb/post");

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
            return "/thumb/post/" + fileName;
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PostId,Title,ShortContents,Contents,Thumb,Published,CreateDate,Author,AccountId,Tags")] Post post, IFormFile thumb)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra xem người dùng đã tải lên ảnh hay chưa
                if (thumb != null && thumb.Length > 0)
                {
                    // Lưu ảnh và lấy đường dẫn
                    var thumbPath = await SaveImage(thumb);
                    // Lưu đường dẫn vào thuộc tính Thumb của bài viết
                    post.Thumb = thumbPath;
                }

                _context.Add(post);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(post);
        }


        // GET: Admin/ADPosts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            return View(post);
        }

        // POST: Admin/ADPosts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PostId,Title,ShortContents,Contents,Thumb,Published,CreateDate,Author,AccountId,Tags")] Post post, IFormFile thumb)
        {
            if (id != post.PostId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Nếu có tệp ảnh mới được tải lên
                    if (thumb != null && thumb.Length > 0)
                    {
                        // Lưu ảnh mới và lấy đường dẫn
                        var thumbPath = await SaveImage(thumb);
                        // Cập nhật đường dẫn ảnh trong bài viết
                        post.Thumb = thumbPath;
                    }
                    else
                    {
                        // Giữ lại đường dẫn ảnh cũ nếu không có ảnh mới được tải lên
                        var existingPost = await _context.Posts.AsNoTracking().FirstOrDefaultAsync(p => p.PostId == id);
                        post.Thumb = existingPost?.Thumb;
                    }

                    _context.Update(post);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostExists(post.PostId))
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
            return View(post);
        }


        // GET: Admin/ADPosts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .FirstOrDefaultAsync(m => m.PostId == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // POST: Admin/ADPosts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post != null)
            {
                _context.Posts.Remove(post);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PostExists(int id)
        {
            return _context.Posts.Any(e => e.PostId == id);
        }
    }
}
