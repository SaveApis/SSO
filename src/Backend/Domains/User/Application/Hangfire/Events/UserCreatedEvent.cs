using SaveApis.Core.Infrastructure.Hangfire.Events;

namespace Backend.Domains.User.Application.Hangfire.Events;

public record UserCreatedEvent(Guid Id) : IEvent
{
    public override string ToString()
    {
        return Id.ToString();
    }
}
