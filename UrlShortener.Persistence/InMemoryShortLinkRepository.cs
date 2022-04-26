using System.Collections.Concurrent;

namespace UrlShortener.Persistence;

public class InMemoryShortLinkRepository : IShortLinkRepository
{
    private static readonly ConcurrentDictionary<string, ShortLink> ShortLinks = new();

    public Task SaveShortLink(ShortLink shortLink)
    {
        if (!ShortLinks.TryAdd(shortLink.Identifier, shortLink))
        {
            throw new DuplicateIdentifierException(shortLink.Identifier);
        }

        return Task.CompletedTask;
    }

    public Task<List<ShortLink>> GetLinks()
    {
        return Task.FromResult(ShortLinks.Values.ToList());
    }

    public Task<ShortLink?> GetLink(string identifier)
    {
        ShortLinks.TryGetValue(identifier, out var shortLink);
        return Task.FromResult(shortLink);
    }

    public Task DeleteLink(string identifier)
    {
        // TODO add some feedback
        ShortLinks.TryRemove(identifier, out _);
        return Task.CompletedTask;
    }
}