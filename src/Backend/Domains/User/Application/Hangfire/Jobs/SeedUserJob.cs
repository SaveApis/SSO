using Backend.Domains.User.Application.Mediator.Commands.CreateUser;
using Backend.Domains.User.Application.Mediator.Queries.GetUsers;
using Backend.Domains.User.Domain;
using Backend.Domains.User.Domain.DTO;
using MediatR;
using SaveApis.Core.Application.Hangfire;
using SaveApis.Core.Application.Hangfire.Events;
using SaveApis.Core.Infrastructure.Extensions;
using SaveApis.Core.Infrastructure.Hangfire.Attributes;
using SaveApis.Core.Infrastructure.Hangfire.Jobs;
using ILogger = Serilog.ILogger;

namespace Backend.Domains.User.Application.Hangfire.Jobs;

[HangfireQueue(HangfireQueue.High)]
public class SeedUserJob(ILogger logger, IMediator mediator) : BaseJob<MigrationCompletedEvent>(logger)
{
    public override async Task<bool> CanExecute(MigrationCompletedEvent @event)
    {
        var result = await mediator.Send(new GetUsersQuery()).ConfigureAwait(false);

        return result.IsSuccess && !result.Value.Any(it => it.IsInitialUser);
    }

    [HangfireJobName("Seed initial user")]
    public override async Task RunAsync(MigrationCompletedEvent @event, CancellationToken cancellationToken)
    {
        var dto = new UserCreateDto
        {
            FirstName = "Developer",
            LastName = "Developer",
            UserName = "developer",
            Email = "developer@saveapis.com",
        };

        var result = await mediator.Send(new CreateUserCommand(dto, SsoRole.Developer, true), cancellationToken).ConfigureAwait(false);
        result.ThrowIfFailed();
    }
}
