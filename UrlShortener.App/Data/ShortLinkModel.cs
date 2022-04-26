using System.ComponentModel.DataAnnotations;

namespace UrlShortener.App.Data;

public class ShortLinkModel
{
    [Required]
    [RegularExpression("(ftp|http|https):\\/\\/[^ \"]+$",ErrorMessage = "It must be a valid url")]
    public string Url { get; set; }
}