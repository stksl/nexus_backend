namespace Nexus.WebApi.Dtos;

public record UpdatePostRequest 
{
    public string Headline {get; init;} 
    public string Content {get; init;}
    public IEnumerable<string> Tags {get; init;}

    public UpdatePostRequest(string headline, string content, IEnumerable<string> tags)
    {
        Headline = headline;
        Content = content;
        Tags = tags.Select(t => t.Trim().ToLowerInvariant());
    }
}