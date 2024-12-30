using Backend.Domains.Mail.Infrastructure.Clients;

namespace Backend.Domains.Mail.Application.Clients;

public class MailClientFactory(IConfiguration configuration) : IMailClientFactory
{
    public IMailClient CreateClient()
    {
        var host = configuration["smtp_host"] ?? "localhost";
        var port = int.TryParse(configuration["smtp_port"] ?? "25", out var p) ? p : 25;
        var userName = configuration["smtp_username"] ?? "user";
        var password = configuration["smtp_password"] ?? "password";
        var enableSsl = bool.TryParse(configuration["smtp_enable_ssl"] ?? "false", out var e) && e;

        return new MailClient(host, port, userName, password, enableSsl);
    }
}
