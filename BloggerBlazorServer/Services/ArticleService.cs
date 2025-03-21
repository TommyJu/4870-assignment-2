using System;
using BloggerBlazorServer.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace BloggerBlazorServer.Services;

public class ArticleService(ApplicationDbContext _context, AuthenticationStateProvider _authenticationStateProvider)
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

    public async Task<Article?> InsertArticleAsync(Article article)
    {
        if (!validateArticle(article))
        {
            return null;
        }
        // Get the current user.
        var user = (await _authenticationStateProvider.GetAuthenticationStateAsync()).User;

        article.CreateDate = DateTime.Now;
        article.StartDate = DateTime.Now;
        article.EndDate = DateTime.Now.AddDays(14);
        // Set the ContributorId and Contributor properties.
        article.ContributorId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        article.Contributor = await _context.Users.FindAsync(article.ContributorId);

        _context.Articles.Add(article);
        await _context.SaveChangesAsync();
        return article;
    }

    public async Task<Article?> UpdateArticleAsync(Article article)
    {
        if (!validateArticle(article))
        {
            return null;
        }
        _context.Entry(article).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return article;
    }

    public async Task<Article?> DeleteArticleAsync(int articleId)
    {
        var article = await _context.Articles.FindAsync(articleId);
        if (article is not null)
        {
            _context.Articles.Remove(article);
            await _context.SaveChangesAsync();
        }
        return article;
    }

    public bool validateArticle(Article article)
    {
        if (string.IsNullOrWhiteSpace(article.Title) || string.IsNullOrWhiteSpace(article.Body))
        {
            return false;
        }
        return true;
    }
}