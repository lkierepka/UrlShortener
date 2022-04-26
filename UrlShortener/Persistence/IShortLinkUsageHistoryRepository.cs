using UrlShortener.Models;

namespace UrlShortener.Persistence;

public interface IShortLinkUsageHistoryRepository
{
    Task RecordUsage(ShortLinkUsageHistory usageHistory);
}