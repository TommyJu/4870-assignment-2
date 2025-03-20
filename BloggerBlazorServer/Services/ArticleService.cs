using System;
using BloggerBlazorServer.Data;
using Microsoft.EntityFrameworkCore;

namespace BloggerBlazorServer.Services;

public class ArticleService(ApplicationDbContext _context)
{
    // Fetch articles asynchronously and eagerly load the Contributor navigation property.
    public async Task<List<Article>> GetArticlesAsync()
    {
        return await _context.Articles
        .Include(a => a.Contributor)
        .ToListAsync();
    }

    public async Task<Article?> GetArticleByIdAsync(int articleId)
    {
        return await _context.Articles
            .Include(a => a.Contributor)
            .FirstOrDefaultAsync(a => a.ArticleId == articleId);
    }
}