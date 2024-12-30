using System.Net;
using System.Net.Mail;
using Backend.Domains.Mail.Infrastructure.Clients;
using Backend.Domains.User.Domain.VO;

namespace Backend.Domains.Mail.Application.Clients;

public class MailClient(string host, int port, string userName, string password, bool enableSsl) : IMailClient
{
    public async Task SendTemplate(string template, Email email, Dictionary<string, object> properties,
        CancellationToken cancellationToken = default)
    {
        var templatePath = Path.Combine("Resources", "Templates", "Mail", $"{template}.html");
        var templateContent = await File.ReadAllTextAsync(templatePath, cancellationToken).ConfigureAwait(false);
        templateContent = properties.Aggregate(templateContent,
            (current, property) => current.Replace($"[{property.Key}]", property.Value.ToString()));

        using var client = CreateClient();

        var message = new MailMessage
        {
            From = new MailAddress(userName),
            Subject = template,
            Body = templateContent,
            IsBodyHtml = true,
        };

        message.To.Add(email.Value);

        await client.SendMailAsync(message, cancellationToken).ConfigureAwait(false);
    }

    private SmtpClient CreateClient()
    {
        return new SmtpClient(host, port)
        {
            Credentials = new NetworkCredential(userName, password),
            EnableSsl = enableSsl,
        };
    }
}
