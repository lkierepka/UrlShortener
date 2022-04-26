using System.Threading.Tasks;
using Shouldly;
using UrlShortener.Persistence;
using Xunit;

namespace UrlShortener.Tests;

public class UrlShortenerServiceTests
{
    private readonly UrlShortenerService _subject;
    private readonly InMemoryShortLinkRepository _inMemoryShortLinkRepository;

    public UrlShortenerServiceTests()
    {
        _inMemoryShortLinkRepository = new InMemoryShortLinkRepository();
        _subject = new UrlShortenerService(new IdentifierGenerator(),
            _inMemoryShortLinkRepository, _inMemoryShortLinkRepository);
    }

    [Fact]
    public async Task ShortenLinkShouldPersist()
    {
        var shortLink = await _subject.ShortenLink(new ShortenLinkRequest { Url = "https://google.com" });
        (await _inMemoryShortLinkRepository.GetLink(shortLink.Identifier)).ShouldNotBeNull();
    }

    [Fact]
    public async Task RedirectShouldReturnLongUrl()
    {
        var url = "https://google.com";
        var shortLink = await _subject.ShortenLink(new ShortenLinkRequest { Url = url });
        var redirect = await _subject.Redirect(shortLink.Identifier);
        redirect.ShouldBe(url);
    }

    [Fact]
    public async Task RedirectShouldIncrementUsage()
    {
        var shortLink = await _subject.ShortenLink(new ShortenLinkRequest { Url = "https://google.com" });
        await _subject.Redirect(shortLink.Identifier);
        (await _inMemoryShortLinkRepository.GetLink(shortLink.Identifier))!.UsageCount.ShouldBe(1);
    }

    [Fact]
    public async Task DeleteShouldRemoveFromPersistence()
    {
        var shortLink = await _subject.ShortenLink(new ShortenLinkRequest { Url = "https://google.com" });
        await _subject.DeleteLink(shortLink.Identifier);
        (await _inMemoryShortLinkRepository.GetLink(shortLink.Identifier)).ShouldBeNull();
    }

    [Fact]
    public async Task GetLinksShouldReturnAll()
    {
        var oldLinks = await _subject.GetShortLinks();
        await _subject.ShortenLink(new ShortenLinkRequest { Url = "https://google.com" });
        await _subject.ShortenLink(new ShortenLinkRequest { Url = "https://google.com" });
        var newLinks = await _subject.GetShortLinks();
        newLinks.Count.ShouldBe(oldLinks.Count + 2);
    }
}