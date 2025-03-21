namespace Nexus.Application;

public class AuthException : Exception 
{
    public AuthException(string? message = null) : base(message)
    {
        
    }
}