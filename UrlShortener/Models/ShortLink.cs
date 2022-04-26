namespace UrlShortener.Models;

public record ShortLink(string Identifier, string LongUrl, DateTime Timestamp, int UsageCount);