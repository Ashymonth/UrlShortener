using Microsoft.EntityFrameworkCore;
using UrlShortener.Models;

namespace UrlShortener.Services;

public interface IShortUrlRepository
{
    Task<ShortUrl> CreateShortUrlAsync(ShortUrl url, CancellationToken ct = default);

    Task<ShortUrl?> GetOriginalUrlAsync(Guid tokenId, CancellationToken ct = default);

    Task<ShortUrl?> GetShortUrlByUrlAsync(string url, CancellationToken ct = default);
}

public class ShortUrlRepository : IShortUrlRepository
{
    private readonly ApplicationDbContext _dbContext;

    public ShortUrlRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ShortUrl> CreateShortUrlAsync(ShortUrl url, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(url);

        await _dbContext.ShortUrls.AddAsync(url, ct);
        await _dbContext.SaveChangesAsync(ct);

        return url;
    }

    public async Task<ShortUrl?> GetOriginalUrlAsync(Guid tokenId, CancellationToken ct = default)
    {
        return await _dbContext.ShortUrls.FirstOrDefaultAsync(url => url.Id == tokenId, ct);
    }

    public async Task<ShortUrl?> GetShortUrlByUrlAsync(string url, CancellationToken ct = default)
    {
        return await _dbContext.ShortUrls.FirstOrDefaultAsync(shortUrl => shortUrl.OriginalUrl == url, ct);
    }
}