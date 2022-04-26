using System.Collections.Concurrent;
using UrlShortener.Exceptions;
using UrlShortener.Models;

namespace UrlShortener.Persistence;

public class InMemoryShortLinkRepository : IShortLinkRepository, IShortLinkUsageHistoryRepository
{
    private static readonly ConcurrentDictionary<string, ShortLink> ShortLinks = new();
    private static readonly ConcurrentDictionary<string, ConcurrentBag<ShortLinkUsageHistory>> UsageHistory = new();

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
        return Task.FromResult(ShortLinks.Values
            .Select(link =>
            {
                UsageHistory.TryGetValue(link.Identifier, out var usage);
                return link with { UsageCount = usage?.Count ?? 0 };
            }).ToList());
    }

    public Task<ShortLink?> GetLink(string identifier)
    {
        ShortLinks.TryGetValue(identifier, out var shortLink);
        if (shortLink is null)
            return Task.FromResult<ShortLink?>(null);
        UsageHistory.TryGetValue(identifier, out var usage);
        return Task.FromResult<ShortLink?>(shortLink with { UsageCount = usage?.Count ?? 0 });
    }

    public Task DeleteLink(string identifier)
    {
        ShortLinks.TryRemove(identifier, out _);
        UsageHistory.TryRemove(identifier, out _);
        return Task.CompletedTask;
    }

    public Task RecordUsage(ShortLinkUsageHistory usageHistory)
    {
        UsageHistory.AddOrUpdate(usageHistory.ShortLinkIdentifier,
            _ => new ConcurrentBag<ShortLinkUsageHistory> { usageHistory },
            (_, bag) =>
            {
                bag.Add(usageHistory);
                return bag;
            });
        return Task.CompletedTask;
    }
}