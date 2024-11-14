namespace UrlShortener.Models;

public class ShortUrl
{
    public Guid Id { get; init; }
    
    /// <summary>
    /// Если есть уверенность, что длинна урла может быть не больше какого-то значения, я бы указал это, чтобы в базе не nvarchar(max) было
    /// </summary>
    public string OriginalUrl { get; init; } = null!;
}