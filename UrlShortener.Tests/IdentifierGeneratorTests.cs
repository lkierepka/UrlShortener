using Shouldly;
using Xunit;

namespace UrlShortener.Tests;

public class IdentifierGeneratorTests
{
    [Fact]
    public void ShouldGenerateEightCharacterIdentifiers()
    {
        var subject = new IdentifierGenerator();
        subject.GetIdentifier().Length.ShouldBe(8);
    }
}