namespace Backend.Domains.Mail.Infrastructure.Clients;

public interface IMailClientFactory
{
    IMailClient CreateClient();
}
