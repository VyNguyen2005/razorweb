using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using razor09_razorweb.models;

namespace razor09_razorweb.Pages_Blog
{
    public class IndexModel : PageModel
    {
        private readonly razor09_razorweb.models.MyBlogContext _context;

        public IndexModel(razor09_razorweb.models.MyBlogContext context)
        {
            _context = context;
        }

        public IList<Article> Article { get; set; } = default!;
        public int? SoLuong { get; set; }
        // khai báo hằng số quy định hiển thị số nội dung trên 1 trang
        public const int ITEMS_COUNT = 1;
        [BindProperty(SupportsGet = true, Name = "p")]
        public int currentPage { get; set; }
        public int countPages { get; set; }

        public async Task OnGetAsync(string SearchString)
        {
            // Article = await _context.articles.ToListAsync();

            var totalArticle = await _context.articles.CountAsync();
            countPages = (int)Math.Ceiling((double)totalArticle / ITEMS_COUNT);

            if(currentPage < 1){
                currentPage = 1;
            }
            if(currentPage > countPages){
                currentPage = countPages;
            }

            var posts = (from a in _context.articles
                        orderby a.Created descending
                        select a)
                        .Skip((currentPage - 1) * ITEMS_COUNT)
                        .Take(ITEMS_COUNT);

            if (!string.IsNullOrEmpty(SearchString))
            {
                Article = posts.Where(p => p.Title.StartsWith(SearchString)).ToList();

                SoLuong = await posts
                        .Where(p => p.Title.StartsWith(SearchString))
                        .CountAsync();
            }
            else
            {
                Article = await posts.ToListAsync();

                var count = await _context.articles.CountAsync();
                SoLuong = count;
            }

        }
    }
}
