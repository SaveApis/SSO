using Backend.Domains.Common.Persistence.Sql;
using Backend.Domains.User.Application.Hangfire.Events;
using Backend.Domains.User.Application.Mediator.Commands.DeleteUser.Errors;
using Backend.Domains.User.Application.Mediator.Errors;
using Backend.Domains.User.Domain.VO;
using FluentResults;
using MediatR;
using SaveApis.Core.Infrastructure.Mediator.Commands;
using SaveApis.Core.Infrastructure.Persistence.Sql.Manager;

namespace Backend.Domains.User.Application.Mediator.Commands.DeleteUser;

public class DeleteUserCommandHandler(IDbManager manager, IMediator mediator) : ICommandHandler<DeleteUserCommand, UserId>
{
    public async Task<Result<UserId>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        await using var context = manager.Create<DataContext>();

        var user = await context.Users.FindAsync([request.Id], cancellationToken).ConfigureAwait(false);
        if (user is null)
        {
            return new UserNotFoundError(request.Id);
        }

        if (user.IsInitialUser)
        {
            return new UserDeletionError();
        }

        context.Users.Remove(user);
        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        await mediator.Publish(new UserDeletedEvent(user.Id.Value), cancellationToken).ConfigureAwait(false);

        return request.Id;
    }
}
