using System;
using BlogLibrary;

namespace SuperBlogger.Web;

public class ArticleApiClient(HttpClient httpClient)
{
    public async Task<Article[]> GetArticlesAsync(int maxItems = 10, CancellationToken cancellationToken = default)
    {
        List<Article>? articles = null;

        await foreach (var article in httpClient.GetFromJsonAsAsyncEnumerable<Article>("/articles", cancellationToken))
        {
            if (articles?.Count >= maxItems)
            {
                break;
            }
            if (article is not null)
            {
                articles ??= [];
                articles.Add(article);
            }
        }

        return articles?.ToArray() ?? [];
    }

    public async Task<Article?> GetArticleByIdAsync(int articleId)
    {
        return await httpClient.GetFromJsonAsync<Article>($"/articles/{articleId}");
    }
};