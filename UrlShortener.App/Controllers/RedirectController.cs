using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace UrlShortener.App.Controllers;

[Route("")]
public class RedirectController : Controller
{
    private readonly UrlShortenerService _urlShortenerService;
    private readonly ILogger<RedirectController> _logger;

    public RedirectController(UrlShortenerService urlShortenerService, ILogger<RedirectController> logger)
    {
        _urlShortenerService = urlShortenerService;
        _logger = logger;
    }

    [HttpGet("/{identifier:length(8)}")]
    public async Task<IActionResult> RedirectToLong(string identifier)
    {
        // TODO Save/Log info about request

        var shortLink = await _urlShortenerService.Redirect(identifier);
        var redirectTo = shortLink ?? "/not-found";
        _logger.LogInformation("Redirecting client {ClientIp} from {ShortUrl} to {TargetUrl}",
            HttpContext.Connection.RemoteIpAddress, Request.GetDisplayUrl(),
            Request.Host.ToUriComponent() + redirectTo);
        return RedirectPermanent(redirectTo);
    }
}