namespace UrlShortener;

public class IdentifierGenerator : IIdentifierGenerator
{
    public string GetIdentifier()
    {
        // TODO maybe borrow memory from a pool
        var buffer = new byte[6];
        var random = new Random();
        random.NextBytes(buffer);
        // TODO think about the readability of the url
        return Convert.ToBase64String(buffer).Replace("/", "2").Replace("+", "2").Replace("=", "2");
    }
}