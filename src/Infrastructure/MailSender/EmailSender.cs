using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using Nexus.Application.Abstractions;

namespace Nexus.Infrastructure;

public class EmailSender : IEmailSender
{
    private readonly IConfiguration _config;
    public EmailSender(IConfiguration config)
    {
        _config = config;
    }
    /// <summary>
    /// Schedules a job
    /// </summary>
    public async Task SendMailAsync(string toEmail, string subject, string htmlBody) 
    {
        SmtpClient client = new SmtpClient("smtp.gmail.com", 587) 
        {
            EnableSsl = true,
            Credentials = new NetworkCredential(_config["SMTP:Email"], _config["SMTP:Password"]),
        };

        MailMessage message = new MailMessage(
            from: _config["SMTP:Email"]!, 
            to: toEmail, 
            subject: subject, 
            body: htmlBody) 
        {
            IsBodyHtml = true
        };
        
        await client.SendMailAsync(message);
    }
}