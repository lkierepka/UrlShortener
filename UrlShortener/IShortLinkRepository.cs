namespace UrlShortener;

public interface IShortLinkRepository
{
    public Task SaveShortLink(ShortLink shortLink);
    Task<List<ShortLink>> GetLinks();
    Task<ShortLink?> GetLink(string identifier);
    Task DeleteLink(string identifier);
}