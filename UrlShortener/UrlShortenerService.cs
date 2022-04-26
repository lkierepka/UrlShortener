namespace UrlShortener;

public class UrlShortenerService
{
    private readonly IIdentifierGenerator _identifierGenerator;
    private readonly IShortLinkRepository _shortLinkRepository;

    public UrlShortenerService(IIdentifierGenerator identifierGenerator, IShortLinkRepository shortLinkRepository)
    {
        _identifierGenerator = identifierGenerator;
        _shortLinkRepository = shortLinkRepository;
    }

    public async Task<ShortLink> ShortenLink(ShortenLinkRequest request)
    {
        Validate(request);
        var shortLink = new ShortLink
        {
            Identifier = _identifierGenerator.GetIdentifier(),
            LongUrl = request.Url,
            Timestamp = DateTime.UtcNow
        };
        await _shortLinkRepository.SaveShortLink(shortLink);
        return shortLink;
    }

    public Task<List<ShortLink>> GetShortLinks()
    {
        return _shortLinkRepository.GetLinks();
    }

    public Task<ShortLink?> GetShortLink(string identifier)
    {
        return _shortLinkRepository.GetLink(identifier);
    }

    public Task DeleteLink(string identifier)
    {
        return _shortLinkRepository.DeleteLink(identifier);
    }

    private void Validate(ShortenLinkRequest request)
    {
        // TODO Validate url
    }
}