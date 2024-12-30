using Backend.Domains.User.Domain.VO;

namespace Backend.Domains.Mail.Infrastructure.Clients;

public interface IMailClient
{
    Task SendTemplate(string template, Email email, Dictionary<string, object> properties, CancellationToken cancellationToken = default);
}
