using UrlShortener.Models;

namespace UrlShortener.Services;

public interface IShortUrlService
{
    Task<ShortUrl> CreateShortUrlAsync(string url, CancellationToken ct = default);
    Task<ShortUrl?> GetOriginalUrlAsync(Guid id, CancellationToken ct = default);
}

public class ShortUrlService : IShortUrlService
{
    private readonly IShortUrlRepository _repository;
    private readonly IQrCodeService _qrCodeService;
    private readonly ILogger<ShortUrlService> _logger;

    public ShortUrlService(IShortUrlRepository repository, ILogger<ShortUrlService> logger, IQrCodeService qrCodeService)
    {
        _repository = repository;
        _logger = logger;
        _qrCodeService = qrCodeService;
    }

    public async Task<ShortUrl> CreateShortUrlAsync(string url, CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(url);

        var existedUrl = await _repository.GetShortUrlByUrlAsync(url, ct);
        if (existedUrl is not null)
        {
            return existedUrl;
        }
        
        var urlId = Guid.NewGuid();
        var qrCodePath = _qrCodeService.GenerateQrCode(urlId, url);
        try
        {
            var shortUrl = new ShortUrl
            {
                Id = urlId,
                OriginalUrl = url,
            };

            return await _repository.CreateShortUrlAsync(shortUrl, ct);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unable to create short url");
            _qrCodeService.DeleteQrCode(qrCodePath);
            throw;
        }
    }

    public async Task<ShortUrl?> GetOriginalUrlAsync(Guid id, CancellationToken ct = default)
    {
        return await _repository.GetOriginalUrlAsync(id, ct);
    }
}