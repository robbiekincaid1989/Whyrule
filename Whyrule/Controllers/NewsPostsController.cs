using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Whyrule.Data;
using Whyrule.Models;

namespace Whyrule.Controllers
{
    public class NewsPostsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public NewsPostsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public string GetUserName()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get user's ID.
            var userName = User.FindFirstValue(ClaimTypes.Name);
            return userName;
        }

        // GET: NewsPosts
        public async Task<IActionResult> Index()
        {
              return _context.NewsPost != null ? 
                          View(await _context.NewsPost.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.NewsPost'  is null.");
        }

        // GET: NewsPosts/ShowSearchForm
        public async Task<IActionResult> ShowSearchForm()
        {
            return View();
        }

        // POST: NewsPosts/ShowSearchResults
        public async Task<IActionResult> ShowSearchResults(string SearchTerm)
        {
            return View("Index", await _context.NewsPost.Where(n => n.Title.Contains(SearchTerm)).ToListAsync());
        }

        // GET: NewsPosts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.NewsPost == null)
            {
                return NotFound();
            }

            var newsPost = await _context.NewsPost
                .FirstOrDefaultAsync(m => m.Id == id);
            if (newsPost == null)
            {
                return NotFound();
            }

            return View(newsPost);
        }

        // GET: NewsPosts/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: NewsPosts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,PostDate,Content,Tags,Author")] NewsPost newsPost)
        {
            if (ModelState.IsValid)
            {
                _context.Add(newsPost);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(newsPost);
        }

        // GET: NewsPosts/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.NewsPost == null)
            {
                return NotFound();
            }

            var newsPost = await _context.NewsPost.FindAsync(id);
            if (newsPost == null)
            {
                return NotFound();
            }
            return View(newsPost);
        }

        // POST: NewsPosts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,PostDate,Content,Tags")] NewsPost newsPost)
        {
            if (id != newsPost.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(newsPost);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NewsPostExists(newsPost.Id))
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
            return View(newsPost);
        }

        // GET: NewsPosts/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.NewsPost == null)
            {
                return NotFound();
            }

            var newsPost = await _context.NewsPost
                .FirstOrDefaultAsync(m => m.Id == id);
            if (newsPost == null)
            {
                return NotFound();
            }

            return View(newsPost);
        }

        // POST: NewsPosts/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.NewsPost == null)
            {
                return Problem("Entity set 'ApplicationDbContext.NewsPost'  is null.");
            }
            var newsPost = await _context.NewsPost.FindAsync(id);
            if (newsPost != null)
            {
                _context.NewsPost.Remove(newsPost);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NewsPostExists(int id)
        {
          return (_context.NewsPost?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
