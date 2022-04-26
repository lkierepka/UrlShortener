namespace UrlShortener;

public class DuplicateIdentifierException : Exception
{
    public readonly string Identifier;

    public DuplicateIdentifierException(string identifier) : base($"Identifier {identifier} already exists")
    {
        Identifier = identifier;
    }
}