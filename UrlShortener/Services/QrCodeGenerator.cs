using IronBarCode;

namespace UrlShortener.Services;

public interface IQrCodeService
{
    string GenerateQrCode(Guid urlId, string url);

    void DeleteQrCode(string path);

    Stream? GetQrCode(Guid qrCodeId);
}

/// <summary>
/// По хорошему надо разбить на 2 сервиса, чтобы солид не нарушать (FileService,OrCodeGenerator), но тут мелкое приложение.
/// </summary>
public class QrCodeService : IQrCodeService
{
    private const string QrCodeDirectory = "QrCodes";
 
    public string GenerateQrCode(Guid urlId, string url)
    {
        var code = QRCodeWriter.CreateQrCode(url);

        var qrCodePath = Path.Combine(QrCodeDirectory, urlId.ToString());

        code.SaveAsJpeg(qrCodePath);

        return qrCodePath;
    }

    public void DeleteQrCode(string path)
    {
        if (File.Exists(path))
        {
            File.Delete(path);
        }

        File.Delete(path);
    }

    public Stream? GetQrCode(Guid qrCodeId)
    {
        var qrCodePath = Path.Combine(QrCodeDirectory, qrCodeId.ToString());

        return File.Exists(qrCodePath) ? File.OpenRead(qrCodePath) : null;
    }
}