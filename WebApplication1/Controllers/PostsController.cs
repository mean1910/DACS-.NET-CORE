using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
using X.PagedList;

namespace WebApplication1.Controllers
{
    public class PostsController : Controller
    {
        private readonly ThaoDuocMarketContext _context;

        public PostsController(ThaoDuocMarketContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int? page)
        {
            int pageNumber = page ?? 1; // Nếu không có trang nào được chỉ định, mặc định là trang 1
            int pageSize = 5; // Số lượng item mỗi trang
            var posts = await _context.Posts.OrderBy(p => p.CreateDate).ToPagedListAsync(pageNumber, pageSize);
            return View(posts);
        }

        // GET: Posts/Details/5
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
    }
}