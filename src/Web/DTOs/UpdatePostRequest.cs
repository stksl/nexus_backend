namespace Nexus.WebApi.Dtos;

public record UpdatePostRequest(int PostId, string Headline, string Content, IEnumerable<string> Tags);