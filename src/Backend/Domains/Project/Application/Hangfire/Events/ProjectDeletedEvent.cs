using SaveApis.Core.Infrastructure.Hangfire.Events;

namespace Backend.Domains.Project.Application.Hangfire.Events;

public record ProjectDeletedEvent(string Id) : IEvent;
