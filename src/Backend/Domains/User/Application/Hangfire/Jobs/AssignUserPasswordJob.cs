using Backend.Domains.User.Application.Hangfire.Events;
using Backend.Domains.User.Application.Mediator.Commands.AssignUserPassword;
using Backend.Domains.User.Application.Mediator.Queries.GetUser;
using Backend.Domains.User.Domain.VO;
using MediatR;
using PasswordGenerator;
using SaveApis.Core.Application.Hangfire;
using SaveApis.Core.Infrastructure.Extensions;
using SaveApis.Core.Infrastructure.Hangfire.Attributes;
using SaveApis.Core.Infrastructure.Hangfire.Jobs;
using ILogger = Serilog.ILogger;

namespace Backend.Domains.User.Application.Hangfire.Jobs;

[HangfireQueue(HangfireQueue.High)]
public class AssignUserPasswordJob(ILogger logger, IMediator mediator) : BaseJob<UserCreatedEvent>(logger)
{
    public override async Task<bool> CanExecute(UserCreatedEvent @event)
    {
        var result = await mediator.Send(new GetUserByIdQuery(UserId.From(@event.Id))).ConfigureAwait(false);

        return result.IsSuccess && string.IsNullOrEmpty(result.Value.Hash);
    }

    [HangfireJobName("{0}: Assign user password")]
    public override async Task RunAsync(UserCreatedEvent @event, CancellationToken cancellationToken)
    {
        var generator = new Password(true, true, true, true, 16);
        string? password;

        do
        {
            password = generator.Next();
        }
        while (string.IsNullOrEmpty(password));

        var result = await mediator.Send(new AssignUserPasswordCommand(UserId.From(@event.Id), password), cancellationToken)
            .ConfigureAwait(false);
        result.ThrowIfFailed();
    }
}
