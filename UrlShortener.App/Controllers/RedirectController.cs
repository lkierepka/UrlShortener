using Microsoft.AspNetCore.Mvc;

namespace UrlShortener.App.Controllers;

[Route("")]
public class RedirectController : Controller
{
    private readonly UrlShortenerService _urlShortenerService;

    public RedirectController(UrlShortenerService urlShortenerService)
    {
        _urlShortenerService = urlShortenerService;
    }

    [HttpGet("/{identifier:length(8)}")]
    public async Task<IActionResult> RedirectToLong(string identifier)
    {
        // TODO Save info about request
        var shortLink = await _urlShortenerService.GetShortLink(identifier);
        return RedirectPermanent(shortLink is null ? "/not-found" : shortLink.LongUrl);
    }
}