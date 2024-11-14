using Microsoft.AspNetCore.Mvc;
using UrlShortener.Services;

namespace UrlShortener.Controllers;

[ApiController]
[Route("api/qrcodes")]
public class QrCodesController : ControllerBase
{
    private const string QrCodeFormat = "image/jpeg";

    private readonly IQrCodeService _qrCodeService;

    public QrCodesController(IQrCodeService qrCodeService)
    {
        _qrCodeService = qrCodeService;
    }

    [HttpGet("qrCode/{urlId:guid}")]
    public IActionResult GetQrCode(Guid urlId, CancellationToken ct)
    {
        var result = _qrCodeService.GetQrCode(urlId);

        if (result is null)
        {
            return NotFound();
        }

        return File(result, QrCodeFormat);
    }
}