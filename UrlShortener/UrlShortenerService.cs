namespace UrlShortener;

public class UrlShortenerService
{
    private readonly IIdentifierGenerator _identifierGenerator;
    private readonly IShortLinkRepository _shortLinkRepository;
    private readonly IShortLinkUsageHistoryRepository _usageHistoryRepository;

    public UrlShortenerService(IIdentifierGenerator identifierGenerator, IShortLinkRepository shortLinkRepository, IShortLinkUsageHistoryRepository usageHistoryRepository)
    {
        _identifierGenerator = identifierGenerator;
        _shortLinkRepository = shortLinkRepository;
        _usageHistoryRepository = usageHistoryRepository;
    }

    public async Task<ShortLink> ShortenLink(ShortenLinkRequest request)
    {
        Validate(request);
        var shortLink = new ShortLink(_identifierGenerator.GetIdentifier(), request.Url, DateTime.UtcNow, 0);
        await _shortLinkRepository.SaveShortLink(shortLink);
        return shortLink;
    }

    public Task<List<ShortLink>> GetShortLinks()
    {
        return _shortLinkRepository.GetLinks();
    }

    public async Task<string?> Redirect(string identifier)
    {
        var link = await _shortLinkRepository.GetLink(identifier);
        if (link is not null)
            await _usageHistoryRepository.RecordUsage(new ShortLinkUsageHistory(identifier));
        return link?.LongUrl;
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