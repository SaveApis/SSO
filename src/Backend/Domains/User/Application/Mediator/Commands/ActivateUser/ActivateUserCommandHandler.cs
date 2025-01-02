using Backend.Domains.Common.Persistence.Sql;
using Backend.Domains.User.Application.Hangfire.Events;
using Backend.Domains.User.Application.Mediator.Errors;
using Backend.Domains.User.Domain.VO;
using FluentResults;
using MediatR;
using SaveApis.Core.Infrastructure.Mediator.Commands;
using SaveApis.Core.Infrastructure.Persistence.Sql.Manager;

namespace Backend.Domains.User.Application.Mediator.Commands.ActivateUser;

public class ActivateUserCommandHandler(IDbManager manager, IMediator mediator) : ICommandHandler<ActivateUserCommand, UserId>
{
    public async Task<Result<UserId>> Handle(ActivateUserCommand request, CancellationToken cancellationToken)
    {
        await using var context = manager.Create<DataContext>();

        var user = await context.Users.FindAsync([request.Id], cancellationToken).ConfigureAwait(false);
        if (user is null)
        {
            return new UserNotFoundError(request.Id);
        }

        user.WithActive(true);
        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        await mediator.Publish(new UserChangedEvent(user.Id.Value), cancellationToken).ConfigureAwait(false);

        return request.Id;
    }
}
