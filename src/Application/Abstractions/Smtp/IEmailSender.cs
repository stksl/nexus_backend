namespace Nexus.Application.Abstractions;

public interface IEmailSender 
{
    Task SendMailAsync(string email, string subject, string htmlBody);
}