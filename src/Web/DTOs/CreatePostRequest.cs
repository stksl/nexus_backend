namespace Nexus.WebApi.Dtos;

public record CreatePostRequest(string Headline, string Content, IEnumerable<string> Tags);