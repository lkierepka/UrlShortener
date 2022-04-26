namespace UrlShortener;

public interface IShortLinkUsageHistoryRepository
{
    Task RecordUsage(ShortLinkUsageHistory usageHistory);
}