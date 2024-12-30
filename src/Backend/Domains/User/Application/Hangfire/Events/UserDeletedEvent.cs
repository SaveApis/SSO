using SaveApis.Core.Infrastructure.Hangfire.Events;

namespace Backend.Domains.User.Application.Hangfire.Events;

public record UserDeletedEvent(Guid Id) : IEvent;
