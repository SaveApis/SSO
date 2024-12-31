using SaveApis.Core.Infrastructure.Hangfire.Events;

namespace Backend.Domains.User.Application.Hangfire.Events;

public record UserChangedEvent(Guid Id) : IEvent;
