using Microsoft.AspNetCore.Mvc;
using UrlShortener.Services;

namespace UrlShortener.Controllers;

public record CreateShortUrlRequest(string Url);

[ApiController]
[Route("api/shorten")]
public class UrlShortenerController : ControllerBase
{
    private readonly IShortUrlService _urlService;

    public UrlShortenerController(IShortUrlService urlService)
    {
        _urlService = urlService;
    }

    [HttpPost]
    [ProducesResponseType<string>(StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateTokenForUrl(CreateShortUrlRequest request, CancellationToken ct)
    {
        if (!Uri.TryCreate(request.Url, UriKind.Absolute, out _))
        {
            return BadRequest("Invalid URL");   
        }
        
        var result = await _urlService.CreateShortUrlAsync(request.Url, ct);
        return CreatedAtAction(nameof(GetUrlFromToken), new { urlId = result.Id }, result.Id);
    }

    [HttpGet("{urlId:guid}")]
    [ProducesResponseType<string>(StatusCodes.Status308PermanentRedirect)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUrlFromToken(Guid urlId, CancellationToken ct)
    {
        var result = await _urlService.GetOriginalUrlAsync(urlId, ct);

        if (result is null)
        {
            return NotFound();
        }

        return Redirect(result.OriginalUrl);
    }
    

}