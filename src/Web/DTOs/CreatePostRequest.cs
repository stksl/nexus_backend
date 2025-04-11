namespace Nexus.WebApi.Dtos;

public record CreatePostRequest 
{
    public string Headline {get; init;} 
    public string Content {get; init;}
    public IEnumerable<string> Tags {get; init;}

    public CreatePostRequest(string headline, string content, IEnumerable<string> tags)
    {
        Headline = headline;
        Content = content;
        Tags = tags.Select(t => t.Trim().ToLowerInvariant());
    }
}