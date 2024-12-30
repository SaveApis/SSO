using SaveApis.Core.Infrastructure.Hangfire.Events;

namespace Backend.Domains.Mail.Application.Hangfire.Events;

public class SendEmailEvent(string email, string template, Dictionary<string, object> properties) : IEvent
{
    public string Email { get; } = email;
    public string Template { get; } = template;
    public Dictionary<string, object> Properties { get; } = properties;

    public override string ToString()
    {
        return $"{Email} - {Template}";
    }
}
