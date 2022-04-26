namespace UrlShortener;

public class ShortLinkUsageHistory
{
    public Guid Id { get; set; }
    public Guid ShortLinkId { get; set; }
    // TODO
    public string RequestInfo { get; set; }
}