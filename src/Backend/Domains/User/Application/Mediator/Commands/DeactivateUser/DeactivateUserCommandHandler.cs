using Backend.Domains.User.Application.Hangfire.Events;
using Backend.Domains.User.Application.Mediator.Errors;
using Backend.Domains.User.Domain.VO;
using Backend.Persistence.Sql;
using FluentResults;
using MediatR;
using SaveApis.Core.Infrastructure.Mediator.Commands;
using SaveApis.Core.Infrastructure.Persistence.Sql.Manager;

namespace Backend.Domains.User.Application.Mediator.Commands.DeactivateUser;

public class DeactivateUserCommandHandler(IDbManager manager, IMediator mediator) : ICommandHandler<DeactivateUserCommand, UserId>
{
    public async Task<Result<UserId>> Handle(DeactivateUserCommand request, CancellationToken cancellationToken)
    {
        await using var context = manager.Create<DataContext>();

        var user = await context.Users.FindAsync([request.Id], cancellationToken).ConfigureAwait(false);
        if (user is null)
        {
            return new UserNotFoundError(request.Id);
        }

        user.WithActive(false);
        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        await mediator.Publish(new UserChangedEvent(user.Id.Value), cancellationToken).ConfigureAwait(false);

        return request.Id;
    }
}
